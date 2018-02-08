package sample;

import java.util.HashSet;
import org.apache.ignite.Ignite;
import org.apache.ignite.IgniteCache;
import org.apache.ignite.Ignition;
import org.apache.ignite.cache.CachePeekMode;
import org.apache.ignite.events.Event;
import org.apache.ignite.events.EventType;
import org.apache.ignite.lang.IgnitePredicate;

import static sample.UploadData.ENTRY_COUNT;
import static sample.UploadData.NEAR_CACHE_SIZE;
import static sample.UploadData.cacheCfg;
import static sample.UploadData.nearCacheCfg;

/**
 * Near Cache Client sample.
 * 1. Please start several ServerNodes
 * 2. Please start UploadData
 * 3. then run this app
 * 4. optionally run ComputeClient
 */
public class NearCacheClient
{
    /**
     * @param args
     * @throws InterruptedException
     */
    public static void main( String[] args ) throws InterruptedException {
        Ignite ignite = Ignition.start("config/example-client.xml");

        final IgniteCache<Integer, Integer> cache = ignite.getOrCreateCache(cacheCfg(), nearCacheCfg());

        ignite.events().localListen(new IgnitePredicate<Event>() {
            @Override public boolean apply(Event event) {
                ignite.log().error("Event: " + event.name() + ", Near Cache size=" + cache.size(CachePeekMode.NEAR) +
                    ", Cache size=" + cache.size(CachePeekMode.PRIMARY));
                return true;
            }
        }, EventType.EVTS_DISCOVERY);

        Thread.sleep(1000);

        while (true) {
            System.out.println("Near Cache size=" + cache.size(CachePeekMode.NEAR) + ", Cache size=" + cache.size());

            long start = System.currentTimeMillis();

            for (Integer i = 0; i < NEAR_CACHE_SIZE; i++)
                cache.get(i);

            long end = System.currentTimeMillis();

            System.out.println(">>> Looked through " + NEAR_CACHE_SIZE + " keys in " + (end - start) + "ms.");
            System.out.println(">>> cache.get(1) result (see compute client): " + cache.get(1));

            try {
                Thread.sleep(5000);
            }
            catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
