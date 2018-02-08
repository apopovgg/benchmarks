package sample;

import java.util.Arrays;
import java.util.Collection;
import org.apache.ignite.Ignite;
import org.apache.ignite.IgniteCache;
import org.apache.ignite.IgniteCompute;
import org.apache.ignite.Ignition;
import org.apache.ignite.lang.IgniteBiTuple;
import org.apache.ignite.lang.IgniteClosure;
import org.apache.ignite.lang.IgniteFuture;
import org.apache.ignite.resources.IgniteInstanceResource;

import static sample.UploadData.NEAR_CACHE_SIZE;
import static sample.UploadData.cacheCfg;
import static sample.UploadData.nearCacheCfg;

/**
 * Compute Client sample.
 * 1. Please start several ServerNodes
 * 2. then run this app
 */
public class ComputeClient
{
    /**
     * @param args
     * @throws InterruptedException
     */
    public static void main( String[] args ) throws InterruptedException {

        try (Ignite ignite = Ignition.start("config/example-client.xml")) {

            ignite.getOrCreateCache(cacheCfg(), nearCacheCfg());

            // run compute locally
            IgniteCompute compute = ignite.compute(ignite.cluster().forLocal());

            Collection<Integer> res = compute.apply(
                new IgniteClosure<IgniteBiTuple<Integer, Integer>, Integer>() {
                    @IgniteInstanceResource
                    Ignite ignite;

                    @Override public Integer apply(IgniteBiTuple<Integer, Integer> input) {
                        System.out.println("Running compute for " + input.toString() + " at thread " + Thread.currentThread().getName());

                        IgniteFuture<Void> fut = ignite.getOrCreateCache(UploadData.CACHE_NAME).putAsync(input.get1(), input.get2());

                        // do some calc
                        Integer calc = input.get2() * input.get2();

                        // wait for put completion
                        fut.get();

                        return calc;
                    }
                },

                // Job parameters. Ignite will create as many jobs as there are parameters.
                Arrays.asList(new IgniteBiTuple<>(1, 2), new IgniteBiTuple<>(10, 20), new IgniteBiTuple<>(100, 200))
            );

            for (Integer r : res)
                System.out.println("Compute result: " + r);
        }
    }
}
