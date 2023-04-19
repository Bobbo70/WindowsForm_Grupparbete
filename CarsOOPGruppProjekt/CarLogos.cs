using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsOOPGruppProjekt
{
    internal class CarLogos
    {

        static public WebClient client = new WebClient();

        static public string defaultLink = "https://www.carlogos.org/img/carlogos.png";
        static public Image defaultLogo = Image.FromStream(new MemoryStream(

                 client.DownloadData(defaultLink)));

        static public string audiLink = "https://www.carlogos.org/car-logos/audi-logo.png";
        static public Image audiLogo = Image.FromStream(new MemoryStream(

            client.DownloadData(audiLink)));
        static public string fordLink = "https://www.carlogos.org/car-logos/ford-logo.png";
        static public Image fordLogo = Image.FromStream(new MemoryStream(

            client.DownloadData(fordLink)));
        static public string renaultLink = "https://www.carlogos.org/car-logos/renault-logo.png";
        static public Image renaultLogo = Image.FromStream(new MemoryStream(
            client.DownloadData(renaultLink)));

        static public string saabLink = "https://www.carlogos.org/car-logos/saab-logo.png";
        static public Image saabLogo = Image.FromStream(new MemoryStream(
            client.DownloadData(saabLink)));

        static public string suzukiLink = "https://www.carlogos.org/car-logos/suzuki-logo.png";
        static public Image suzukiLogo = Image.FromStream(new MemoryStream(client.DownloadData(suzukiLink)));

        static public string volvoLink = "https://www.carlogos.org/car-logos/volvo-logo.png";
        static public Image volvoLogo = Image.FromStream(new MemoryStream(client.DownloadData(volvoLink)));
    }
}
