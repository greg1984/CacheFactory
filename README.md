# CacheFactory

README Authored by Gregory Goldberg on the 28th of May, 2016.
README Last Modified by Gregory Goldberg on the 28th of May, 2016

This is an Open Source Abstracted Generic C# .NET Caching package.

Anyone is free to use this at their will, please give me any positive or negative feedback you may have when using it and enjoy!

The following functionality is included in this package (in no particular order) with descriptions of some of the current/future possible feature developments:
- A Factory class which enables the generation of Generic Caching capabilities for any form of .NET object: ~/CacheFactory.cs
-- Currently developing global Cache capabilities for the Cache Factory
- An interface which enables the extenion of the functionality classes included in this package within external packages: ~/Cachers/Base/ICache.cs
- An abstracted class which includes base functionality for the  which enables developers to extend Caching functionality to external Cache classes: ~/Cachers/Base/ACache.cs
- Generic Cache Item to be used with the Cache Interface/Abstraction as well as the Cache Factory: ~/Cachers/Base/CacheItem.cs
- Generic Cache Item Key to be used with the Cache Interface/Abstraction, the generic Cache Item as well as the Cache Factory: ~/Cachers/Base/CacheItemKey.cs
- A fully implemented First In First Out (FIFO) Cache: ~/Cachers/FirstInFirstOutCache.cs
- A fully implemented First In Last Out (FILO) Cache: ~/Cachers/FirstInLastOutCache.cs
- A Time Based Eviction Strategy class: ~/Cachers/TimeBasedEvictionCache.cs
-- Static Cache Eviction Strategies utilized by the Time Based Eviction Strategy that I am considering converting into an interface: ~/Cachers/CacheEvictor.cs
-- Enumerations of the Eviction Strategies for the TIme Based Eviction Strategy class, will be redundant if the Static Cache Eviction Strategies are changed to an interface: ~/TimeBasedEvictionStrategies.cs

Current State of the BETA branch:
-The Test suite currently contains 65 tests within the test suite which has a coverage of 91%
-- Missing some coverage as retrieving the global cache and the cache naming mechanisms are still under development and I did not want to commit failing unit tests)
-- 221 lines of code covered.
-- 23 lines of code missing coverage.
-- 244 lines of code implemented to get the package to this stage.
-- Conception: 26th of May, 2016
-- Design and initial implementations of Repo, Source code base and Test code base: 27th of May