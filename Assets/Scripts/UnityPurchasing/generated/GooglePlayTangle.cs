// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("VUoxfr0A34bd5qurE+wC+91r9BQwK1DtgjsvAXcQ4xCB7DrWx9mtg6GCGnKuI7rXczfmBw3qqKmRsG8m6qossc0vJtgYvASqeNpTC1TupvGcNwVIk3gNBv7jIesvtkzeb2m5iFrZ19joWtnS2lrZ2dh1j2TN8OdRObzYKv5AhIM5x5BXlq7BP3NJkfdg7MHbmni0ONRIckaUP5LY6M/j1uWVvfRwYllq2tNWVf0K+aBmP9AYdyOOId2k6BNuRJ8JUxU83GMj0VbtevFsWu3FztIN082d8kpOlKUrSs5BmmoM5wUbRspzaEDsY4z7MmeS6FrZ+ujV3tHyXpBeL9XZ2dnd2NuA0fiwxD1B9Gq8AmqWuTCiuRrFQchMeUHRCp8BO9rb2djZ");
        private static int[] order = new int[] { 4,10,13,12,7,10,9,13,12,9,11,11,13,13,14 };
        private static int key = 216;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
