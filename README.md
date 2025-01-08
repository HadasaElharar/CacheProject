# Cache Project

## Description
This project is a simple **in-memory cache system** implemented in C#.  
The cache provides basic functionality such as adding, fetching, updating, and deleting cached items.  
It also includes **TTL (time-to-live)** for each cache entry, allowing automatic cleanup of expired entries.

## Features
- **Add Entries**: Add new key-value pairs to the cache.
- **Fetch Entries**: Retrieve values by their keys.
- **Update Entries**: Update the value of an existing key.
- **Delete Entries**: Remove entries from the cache.
- **Persistence**: Cache data is saved to a file to ensure durability even after the application restarts.
- **Automatic Cleanup**: Automatically removes expired entries to maintain a clean state.

## Getting Started

### Prerequisites
- .NET 6 or higher
- Basic knowledge of C# programming
- Git (optional, for version control)

### Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/your-username/cache-project.git
   cd cache-project
