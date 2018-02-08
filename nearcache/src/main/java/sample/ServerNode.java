package sample;

import org.apache.ignite.IgniteException;
import org.apache.ignite.Ignition;

/**
 * just a ServerNode
 */
public class ServerNode {
    /**
     * Starts up an empty node with example compute configuration.
     */
    public static void main(String[] args) throws IgniteException {
        Ignition.start("config/example-server.xml");
    }
}
