using UnityEngine;


namespace Ludus.SDK.Utils
{
    public class Utils : MonoBehaviour
    {
        public static string genRandomNumAsString(uint bytes = 0)
        {

            try
            {
                if (bytes == 0)
                {
                    throw new UnityException("[genRndNumAsStr(): Defina um valor válido à string.");
                }

                string buffer = "";
                string allowedCode = "1234567890";
                System.Random rand = new System.Random();
                uint i = 0;
                while (i < bytes){
                    buffer += allowedCode[rand.Next(0,allowedCode.Length)];
                    i++;
                }

                return buffer;
            }
            catch (UnityException err)
            {
                throw err;
            }
        }
    }
}

