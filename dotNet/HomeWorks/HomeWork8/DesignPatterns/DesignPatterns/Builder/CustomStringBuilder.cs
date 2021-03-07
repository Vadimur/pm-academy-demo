using System;
using System.Diagnostics.Contracts;

namespace DesignPatterns.Builder
{
    public class CustomStringBuilder : ICustomStringBuilder
    {
        private char[] _charsChunk;
        private int _actualNumberOfCharsInChunk;                  // The index in _chunkChars that represent the end of the block
        public CustomStringBuilder()
        {
            _charsChunk = new char[0];
            _actualNumberOfCharsInChunk = _charsChunk.Length;
        }

        public CustomStringBuilder(string text)
        {
            _charsChunk = string.IsNullOrEmpty(text) ? new char[0] : text.ToCharArray();
            _actualNumberOfCharsInChunk = _charsChunk.Length;
        }

        public ICustomStringBuilder Append(string str)
        {
            if (_actualNumberOfCharsInChunk + str.Length >= _charsChunk.Length)
            {
                ExpandChunk(_actualNumberOfCharsInChunk + str.Length);
            }
            
            foreach (char ch in str)
            {
                Append(ch);
            }

            return this;
        }

        public ICustomStringBuilder Append(char ch)
        {
            if (_actualNumberOfCharsInChunk + 1 > _charsChunk.Length)
            {
                ExpandChunk(_actualNumberOfCharsInChunk + 1);
            }
            
            int index = _actualNumberOfCharsInChunk;
            _charsChunk[index] = ch;
            
            _actualNumberOfCharsInChunk++;

            return this;
        }

        public ICustomStringBuilder AppendLine()
        {
            return Append("\n");
            //return Append(Environment.NewLine);
        }

        public ICustomStringBuilder AppendLine(string str)
        {
            Append(str);
            
            return Append("\n");
            //return Append(Environment.NewLine);
        }

        public ICustomStringBuilder AppendLine(char ch)
        {
            Append(ch);
            
            return Append("\n");
            //return Append(Environment.NewLine);
        }

        public string Build()
        {
            Array.Resize(ref _charsChunk, _actualNumberOfCharsInChunk);
            
            return new string(_charsChunk);
        }

        private void ExpandChunk(int newChunkSize)
        {
            if (newChunkSize < _actualNumberOfCharsInChunk)
            {
                throw new ArgumentException("New size cannot be lower than current size", nameof(newChunkSize));
            }
            
            if (newChunkSize == _charsChunk.Length)
            {
                return;
            }
            
            Array.Resize(ref _charsChunk, newChunkSize);
        }
    }
}