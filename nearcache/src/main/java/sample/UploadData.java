package sample;

import org.apache.ignite.Ignite;
import org.apache.ignite.IgniteCache;
import org.apache.ignite.IgniteDataStreamer;
import org.apache.ignite.Ignition;
import org.apache.ignite.cache.CacheMode;
import org.apache.ignite.cache.CacheWriteSynchronizationMode;

import org.apache.ignite.cache.eviction.lru.LruEvictionPolicyFactory;
import org.apache.ignite.configuration.CacheConfiguration;
import org.apache.ignite.configuration.NearCacheConfiguration;

/**
 * Uploads sample data to the cluster
 *
 */
public class UploadData
{
    /** Cache name. */
    public static final String CACHE_NAME = UploadData.class.getSimpleName();

    /** Number of entries to load. */
    public static final int ENTRY_COUNT = 100000;

    /** Number of entries to load. */
    public static final int NEAR_CACHE_SIZE = ENTRY_COUNT / 4;

    /**
     * @param args
     */
    public static void main( String[] args )
    {
        try (Ignite ignite = Ignition.start("config/example-client.xml")) {
            System.out.println();

            // Auto-close cache at the end of the example.
            try (IgniteCache<Integer, Integer> cache = ignite.getOrCreateCache(cacheCfg(), nearCacheCfg())) {
                runStreamer(ignite);
            }
        }
    }

    /**
     * @param ignite Ignite
     */
    private static void runStreamer(Ignite ignite) {
        long start = System.currentTimeMillis();

        try (IgniteDataStreamer<Integer, Integer> stmr = ignite.dataStreamer(CACHE_NAME)) {
            // Configure loader.
            stmr.perNodeBufferSize(1024);
            stmr.perNodeParallelOperations(8);

            for (int i = 0; i < ENTRY_COUNT; i++) {
                stmr.addData(i, i);

                // Print out progress while loading cache.
                if (i > 0 && i % 10000 == 0)
                    System.out.println("Loaded " + i + " keys.");
            }
        }

        long end = System.currentTimeMillis();
        System.out.println(">>> Loaded " + ENTRY_COUNT + " keys in " + (end - start) + "ms.");
    }

    /**
     * @return CacheConfiguration
     */
    public static CacheConfiguration<Integer, Integer> cacheCfg() {
        return new CacheConfiguration<Integer, Integer>()
            .setName(CACHE_NAME)
            .setBackups(0)
            .setCacheMode(CacheMode.REPLICATED)
            .setWriteSynchronizationMode(CacheWriteSynchronizationMode.FULL_SYNC);
    }

    /**
     * @return NearCacheConfiguration
     */
    public static NearCacheConfiguration<Integer, Integer> nearCacheCfg() {
        return new NearCacheConfiguration<Integer, Integer>()
            .setNearEvictionPolicyFactory(new LruEvictionPolicyFactory<Integer, Integer>(NEAR_CACHE_SIZE))
            .setNearStartSize(NEAR_CACHE_SIZE);
    }

}
