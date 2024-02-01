using System;
using System.Windows.Forms;

namespace bookShopping
{
    internal static class Program
    {
        /// <summary> 
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //addImage addImage = new addImage();
            //addImage.MainImage();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HomePage(-1, null));
        }
    }
}