using System;
using System.Collections.Generic;

namespace IndependentWork14
{
    public class FileMetadataCache
    {
        private readonly Dictionary<string, FileMetadata> _cache;

        public FileMetadataCache()
        {
            _cache = new Dictionary<string, FileMetadata>(StringComparer.OrdinalIgnoreCase);
        }

        public int Count => _cache.Count;

        public void Add(string filePath, FileMetadata metadata)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be empty.", nameof(filePath));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            _cache[filePath] = metadata;
        }

        public FileMetadata Get(string filePath)
        {
            if (_cache.TryGetValue(filePath, out var metadata))
            {
                return metadata;
            }

            throw new KeyNotFoundException($"Metadata for '{filePath}' was not found.");
        }

        public bool Remove(string filePath)
        {
            return _cache.Remove(filePath);
        }

        public bool Contains(string filePath)
        {
            return _cache.ContainsKey(filePath);
        }
    }
}