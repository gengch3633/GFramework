// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("gAMNAjKAAwgAgAMDAoSRP8+r+FZ3Xcip+/S8vlNugkSk1GBxr9wYUlj5AGP8Dpmjkpd8hNhXhcG5UmV0qjYIqLUy4UbmMgqR3qtP1PewyrEygAMgMg8ECyiESoT1DwMDAwcCATmoD15/RN6rPFL7FgTdp1uUY0R7zztt7wsFtxGG0+W4dtVIFJTFDDdWDFGeXZsbd6G1UukoV823f7XMRghlP3Juvhiqgd9AqpM9xs3XkmU4H4CoowEY10IQS0uD08ewDodIxFxJ8XYhJ9g6282nYwrIVy+vQ9gu7dU6a9M2TYR8n4JTBlrkxIDiOdOVHkfQzynGptf0GUhmgbF9V44/4GYYkWzu8kfgax73VHNiRb2akv7c6nWPYARAwngk0QABAwID");
        private static int[] order = new int[] { 1,10,9,12,10,12,12,10,13,9,12,13,12,13,14 };
        private static int key = 2;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
