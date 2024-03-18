using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace SchnapsNet.Utils
{
    public class MimeType
    {
        private static readonly byte[] BMP = { 66, 77 };
        private static readonly byte[] DOC = { 208, 207, 17, 224, 161, 177, 26, 225 };
        private static readonly byte[] EXE_DLL = { 77, 90 };
        private static readonly byte[] GIF = { 71, 73, 70, 56 };
        private static readonly byte[] ICO = { 0, 0, 1, 0 };
        private static readonly byte[] JPG = { 255, 216, 255 };
        private static readonly byte[] MP3 = { 255, 251, 48 };
        private static readonly byte[] OGG = { 79, 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] PDF = { 37, 80, 68, 70, 45, 49, 46 };
        private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
        private static readonly byte[] RAR = { 82, 97, 114, 33, 26, 7, 0 };
        private static readonly byte[] SWF = { 70, 87, 83 };
        private static readonly byte[] TIFF = { 73, 73, 42, 0 };
        private static readonly byte[] TORRENT = { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 };
        private static readonly byte[] TTF = { 0, 1, 0, 0, 0 };
        private static readonly byte[] WAV_AVI = { 82, 73, 70, 70 };
        private static readonly byte[] WMV_WMA = { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 };
        private static readonly byte[] ZIP_DOCX = { 80, 75, 3, 4 };


        // public static string DefaultMimeType = DEFAULTMIMETYPE


        /// <summary>
        /// GetMimeType
        /// </summary>
        /// <param name="file"><see cref="byte[]">byte[] binary array</see></param>
        /// <param name="fileName">save filename</param>
        /// <returns>detected mime type by binary byte pattern, 
        /// if no specific mime type detect => default application/octet-stream</returns>
        public static string GetMimeType(byte[] file, string fileName)
        {

            string mime = Constants.DEFAULT_MIMETYPE;

            //Ensure that the filename isn't empty or null
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return mime;
            }

            //Get the file extension
            string extension = Path.GetExtension(fileName) == null
                                   ? string.Empty
                                   : Path.GetExtension(fileName).ToUpper();

            //Get the MIME Type
            if (file.Take(2).SequenceEqual(BMP))
            {
                mime = "image/bmp";
            }
            else if (file.Take(8).SequenceEqual(DOC))
            {
                mime = "application/msword";
            }
            else if (file.Take(2).SequenceEqual(EXE_DLL))
            {
                mime = "application/x-msdownload"; //both use same mime type
            }
            else if (file.Take(4).SequenceEqual(GIF))
            {
                mime = "image/gif";
            }
            else if (file.Take(4).SequenceEqual(ICO))
            {
                mime = "image/x-icon";
            }
            else if (file.Take(3).SequenceEqual(JPG))
            {
                mime = "image/jpeg";
            }
            else if (file.Take(3).SequenceEqual(MP3))
            {
                mime = "audio/mpeg";
            }
            else if (file.Take(14).SequenceEqual(OGG))
            {
                if (extension == ".OGX")
                {
                    mime = "application/ogg";
                }
                else if (extension == ".OGA")
                {
                    mime = "audio/ogg";
                }
                else
                {
                    mime = "video/ogg";
                }
            }
            else if (file.Take(7).SequenceEqual(PDF))
            {
                mime = "application/pdf";
            }
            else if (file.Take(16).SequenceEqual(PNG))
            {
                mime = "image/png";
            }
            else if (file.Take(7).SequenceEqual(RAR))
            {
                mime = "application/x-rar-compressed";
            }
            else if (file.Take(3).SequenceEqual(SWF))
            {
                mime = "application/x-shockwave-flash";
            }
            else if (file.Take(4).SequenceEqual(TIFF))
            {
                mime = "image/tiff";
            }
            else if (file.Take(11).SequenceEqual(TORRENT))
            {
                mime = "application/x-bittorrent";
            }
            else if (file.Take(5).SequenceEqual(TTF))
            {
                mime = "application/x-font-ttf";
            }
            else if (file.Take(4).SequenceEqual(WAV_AVI))
            {
                mime = extension == ".AVI" ? "video/x-msvideo" : "audio/x-wav";
            }
            else if (file.Take(16).SequenceEqual(WMV_WMA))
            {
                mime = extension == ".WMA" ? "audio/x-ms-wma" : "video/x-ms-wmv";
            }
            else if (file.Take(4).SequenceEqual(ZIP_DOCX))
            {
                mime = extension == ".DOCX" ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document" : "application/x-zip-compressed";
            }

            return mime;
        }

        /// <summary>
        /// GetFileExtForMimeTypeApache
        /// </summary>
        /// <param name="mimeString">Mime type string in format genericType/specificType, e.g.:
        /// image/bmp
        /// image/gif
        /// image/x-icon
        /// image/jpeg
        /// audio/mpeg
        /// audio/ogg
        /// application/msword
        /// </param>
        /// <returns>extension based on windows / dos rules with 3 <see cref="char"/></returns>
        public static string GetFileExtForMimeTypeApache(string mimeString)
        {
            //Ensure that the mimeString isn't empty or null
            if (string.IsNullOrWhiteSpace(mimeString))
            {
                mimeString = "application/octet-stream";
            }

            //Get the file extension
            switch (mimeString.ToLower())
            {
                case "image/avif": return "avif";
                case "image/bmp": return "bmp";
                case "image/gif": return "gif";
                case "image/ief": return "ief";                
                case "image/jpg": return "jpg";
                case "image/jpeg": return "jpg";                                
                case "image/png": return "png";
                case "image/vnd.adobe.photoshop": return "psd";
                case "image/svg":
                case "image/svg+xml": return "svg";
                case "image/tiff": return "tif";
                case "image/vnd.xiff": return "xif";                
                case "image/x-icon": return "ico";
                case "image/x-pcx": return "pcx";
                case "image/x-pict": return "pic";
                case "image/x-rgb": return "rgb";
                case "image/x-xbitmap": return "xbm";
                case "image/x-xpixmap": return "xpm";
                case "image/xcf": return "xcf";

                case "font/otf": return "otf";
                case "font/ttf": return "ttf";
                case "font/woff": return "woff";
                case "font/woff2": return "woff2";

                case "text/css": return "css";
                case "text/csv": return "csv";
                case "text/html": return "htm";
                case "text/javascript": return "js";
                case "text/plain": return "txt";
                case "text/richtext": return "rtx";
                case "text/x-asm": return "asm";
                case "text/x-c": return "c";
                case "text/x-python": return "py";
                case "text/x-uuencode": return "uu";
                case "text/x-vcalendar": return "vcs";
                case "text/x-vcard": return "vcf";

                case "audio/3gpp": return ".3gp";
                case "audio/3gpp2": return "3g2";
                case "audio/aac": return "aac";
                case "audio/aiff": return "aif";
                case "audio/basic": return "au";
                case "audio/midi": return "mid";
                case "audio/mp4": return "mp4";
                case "audio/mpeg": return "mpg";
                case "audio/ogg": return "oga";                
                case "audio/webm": return "weba";
                case "audio/x-aiff": return "aif";
                case "audio/x-mpegurl": return "m3u";
                case "audio/x-wav": return "wav";
                case "audio/x-ms-wax": return "wax";
                case "audio/x-ms-wma": return "wma";

                case "video/3gpp": return "3gp";
                case "video/3gpp2": return "3g2";
                case "video/mp4": return "mp4";
                case "video/mpeg": return "mpg";                
                case "video/ogg": return "ogg";
                case "video/quicktime": return "mov";
                case "video/vnd.mpegurl": return "m4u";
                case "video/webm": return "webm";
                case "video/x-f4v": return "f4v";
                case "video/x-flv": return "flx";
                case "video/x-m4v": return "m4v";

                case "video/x-msvideo": return "avi";
                case "video/x-ms-wmv": return "wmv";
                case "video/x-sgi-movie": return "movie";

                case "application/atom":
                case "application/atom+xml": return "atom";
                case "application/java-archive": return "jar";
                case "application/json": return "json";
                case "application/mbox": return "mbox";
                case "application/mp4": return "mp4s";
                case "application/msword": return "doc";
                case "application/mxf": return "mxf";
                case "application/ogg": return "ogx";
                case "application/onenote": return "onepkg";
                case "application/pdf": return "pdf";
                case "application/pkcs10": return "p10";
                case "application/pkcs7-mime": return "p7c";
                case "application/pkcs7-signature": return "p7s";
                case "application/pkix-cert": return "cer";
                case "application/pkix-crl": return "crl";
                case "application/pkixcmp": return "pki";
                case "application/postscript": return "ps";
                case "application/rdf":
                case "application/rdf+xml": return "rdf";                
                case "application/rtf": return "rtf";
                case "application/vnd.amazon.ebook": return "azw";
                case "application/vnd.ms-excel": return "xls";
                case "application/vnd.ms-fontobject": return "eot";
                case "application/vnd.ms-project": return "mpp";
                case "application/vnd.ms-powerpoint": return "ppt";
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation": return "pptx";
                case "application/vnd.oasis.opendocument.presentation": return "odp";
                case "application/vnd.oasis.opendocument.text": return "odt";
                case "application/vnd.visio": return "vsd";
                case "application/wasm": return "wasm";
                case "application/x-bittorrent": return "torrent";
                case "application/x-cdf": return "cda";
                case "application/x-freearc": return "arc";
                case "application/x-msaccess": return "mdb";
                case "application/x-msdownload": return "exe";
                case "application/x-font-ttf": return "ttf";
                case "application/xhtml+xml":
                case "application/xhtml": return "xhtml";
                case "application/x-sqlite3": return "sqlite";

                case "application/x-bzip": return "bz";
                case "application/x-bzip2": return "bz2";
                case "application/x-gzip": return "gz";
                case "application/x-tar": return "tar";

                case "application/x-7z-compressed": return "7z";
                case "application/x-rar-compressed": return "rar";
                case "application/x-zip-compressed": return "zip";
                case "application/octet-stream":
                default: break;
            }

            return "oct";
        }

    }

}