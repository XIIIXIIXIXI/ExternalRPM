using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;

namespace ExternalRPM.Modules
{
    class ObjectManager
    {
        public static Offsets.GameObject[] ReadGameObjects()
        {
            List<Offsets.GameObject> output = new List<Offsets.GameObject>();
            long objectManager = Offsets.Instances.GetobjectManager;
            long objectManagerRootNode = Memory.Read<long>(objectManager + Offsets.Object.ObjectMapRoot);
            long count = Memory.Read<long>(objectManager + Offsets.Object.ObjectMapCount);
            List<long> objectAddresses = new List<long>();
            List<long> scannedNodesAddresses = new List<long>();
            ScanNode(objectManagerRootNode, objectAddresses, scannedNodesAddresses);
            foreach (var eachObj in objectAddresses)
            {
                var readObject = Memory.Read<Offsets.GameObject>(eachObj);
                string name = Memory.ReadString(eachObj + Offsets.Object.ObjectName, 20);
                output.Add(readObject);
            }
            return output.ToArray();
        }

        private static unsafe void ScanNode(long addressToScan, List<long> objectAddresses,
            List<long> scannedNodesAddresses)
        {
            if (addressToScan == 0 || scannedNodesAddresses.Contains(addressToScan))
            {
                return;
            }

            var currentNode = Memory.Read<Offsets.ObjectNode>(addressToScan);
            if (currentNode.nodeObject != 0)
            {
                objectAddresses.Add(currentNode.nodeObject);
            }
            scannedNodesAddresses.Add(addressToScan);
            for (int i = 0; i < 3; i++)
            {
                ScanNode(currentNode.nodes[i], objectAddresses, scannedNodesAddresses);
            }
        }
    }
}
