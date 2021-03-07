using System;
using System.IO;
using DepsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DepsWebApp.Filters
{
#pragma warning disable CS1591
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            CustomExceptionModel customExceptionModel = MapExceptionToCustomException(context.Exception);
            
            context.Result = new JsonResult(customExceptionModel);
            context.ExceptionHandled = true;
        }

        private CustomExceptionModel MapExceptionToCustomException(Exception origException) // is it good idea to make it as extension method?
        {
            switch (origException)
            {
                case NullReferenceException _:
                case ArgumentNullException _:
                case ArgumentOutOfRangeException _: 
                case DivideByZeroException _:
                case FileNotFoundException _:    
                case System.Net.Http.HttpRequestException _:
                case System.Text.Json.JsonException _:    
                    return CustomExceptionModel.InternalServerError;
                
                case InvalidOperationException _: // here should be custom exceptions
                    return CustomExceptionModel.BadRequest;
                
                case NotImplementedException  _: // here should be custom exceptions
                    return CustomExceptionModel.FailedRegistration;
                
                default:
                    return CustomExceptionModel.UnknownError;
            }
        }
    }

#pragma warning restore CS1591
}