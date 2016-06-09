# CacheFactory

## README Authored by Gregory Goldberg on the 28th of May, 2016.
## README Last Modified by Gregory Goldberg on the 9th of June, 2016

### This is an Open Source Abstracted Generic C# .NET Caching package.

Anyone is free to use this at their will, please give me any positive or negative feedback you may have when using it and enjoy!

The following functionality is included in this package (in no particular order) with descriptions of some of the current/future possible feature developments:
* A Factory class which enables the generation of Generic Caching capabilities for any form of .NET object: ~/CacheFactory.cs
* A Factory class which manages singleton instances of caches based on cache name (Thread supported): ~/CacheManager.cs
* An interface which enables the extenion of the functionality classes included in this package within external packages: ~/Cachers/Base/ICache.cs
* An abstracted class which includes base functionality for the  which enables developers to extend Caching functionality to external Cache classes: ~/Cachers/Base/ACache.cs
* An abstracted class which includes base functionality for the  which enables developers to extend Caching functionality to external Cache classes, this also has events to which developers can attach their own handlers: ~/Cachers/Base/ACacheWithEvents.cs
* Generic Cache Item to be used with the Cache Interface as well as the Cache Factory: ~/Cachers/Base/ICacheItem.cs
* Generic Cache Item to be used with the Cache Abstraction as well as the Cache Factory: ~/Cachers/Base/ACacheItem.cs
* Generic Cache Item Key to be used with the Cache Interface the generic Cache Item as well as the Cache Factory: ~/Cachers/Base/ICacheItemKey.cs
* Generic Cache Item Key to be used with the Cache Abstraction the generic Cache Item as well as the Cache Factory: ~/Cachers/Base/ACacheItemKey.cs
* A fully implemented abstracted First In First Out (FIFO) Cache: ~/Cachers/FirstInFirstOutCache.cs
* A fully implemented abstracted First In Last Out (FILO) Cache: ~/Cachers/FirstInLastOutCache.cs
* A fully implemented abstracted Least Recently Used (LRU) Cache: ~/Cachers/LeastRecentlyUsedCache.cs
* Static Cache Eviction Strategies: ~/Cachers/CacheEvictor.cs

## Current State of the BETA branch:
* The Test suite currently contains 101 tests within the test suite which has a coverage of 98%
* 519 lines of code covered.
* 13 lines of code missing coverage (Mostly event argument getters).
* 532 lines of code implemented to get the package to this stage.
* Conception: 26th of May, 2016
* Design and initial implementations of Repo, Source code base and Test code base: 27th of May
* Estimated effort spent against project *18 hours*

## Future work intentions:
* A simulator which reads in log files and shows metrics on the potential benefits of implementing  a configured form of caching.
* A way to save and load the cache objects (ORM's/OLEDB/ODBC/File etc)
* Possibly merging the 3 different types of abstracted caches into a single cache class with a Lambda function(s) to qualify the eviction method.
* Configurable logging utilizing log4net.
* Configurable metrics generation.
* Code Wiki
* More (Feel free to submit requests!)