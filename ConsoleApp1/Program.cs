using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        bravodeliver_posEntities db = new bravodeliver_posEntities();
        static void Main(string[] args)
        {
            // new Program().InsertEntriesIntoProduct();
            //  new Program().UpdateEntriesInProduct();

            new Program().InsertIntoBridgeStoreProduct();
        }

        public void InsertIntoBridgeStoreProduct()
        {
            List<string> prodId = db.Products.Select(s =>(s.Korona_ProductId)).ToList();
            var storeId = db.Stores.Where(x => x.City.Contains("Arlington")).Select(x=>x.id).ToList();
            Store_Product sp;
            
            foreach(int item in storeId)
            {
               
                
                foreach(var prdId in prodId)
                {
                    sp = new Store_Product();
                    sp.StoreId = item;
                    sp.ProductId = prdId;

                    db.Store_Product.Add(sp);
                    db.SaveChanges();
                }
            }
            
        }
        public void InsertEntriesIntoProduct()
        {
            //bravodeliver_posEntities db = new bravodeliver_posEntities();
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            Product prods;
            double price = 0;
            using (var reader = new StreamReader(Path.Combine("E:\\FreeLance\\stores\\stores", "\\Arlington.csv")))
            {

                while (!reader.EndOfStream)
                {
                    prods = new Product();

                    var line = reader.ReadLine();

                    string[] values = line.Split(',');
                    
                    if (!string.IsNullOrEmpty(values[12]))
                    {
                        price = Convert.ToDouble(values[12]);
                    }
                    else
                    {
                        price = 0;
                    }
                    // prods.Add(new Product() { Korona_ProductId = splits[0], Name = splits[1], Category = splits[2], Price = price });

                    prods.Korona_ProductId = values[0];
                    prods.Name = values[1];
                    prods.Category = values[5];
                    prods.Price = price;

                    try
                    {
                        db.Products.Add(prods);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }


                }
            }
        }

        public void UpdateEntriesInProduct()
        {
            List<PrductImages> prductImages = new List<PrductImages>();

            var prds = db.Products.ToList();
            Product myProd;
            string[] filesWithoutExt = Directory.GetFiles(@"E:\FreeLance\Farrukh-images\Farrukh-images").Select(file => Path.GetFileNameWithoutExtension(file)).ToArray();
            string[] filesWithExt = Directory.GetFiles(@"E:\FreeLance\Farrukh-images\Farrukh-images").Select(file => Path.GetFileName(file)).ToArray();

            for (int i = 0; i < filesWithoutExt.Length; i++)
            {
                myProd = prds.Where(x => x.Name.ToLower() == filesWithoutExt[i].ToLower()).FirstOrDefault();
                if (myProd != null)
                {
                    myProd.Image = "https://www.bravodelivery.com/images/"+ filesWithExt[i];
                    db.SaveChanges();
                }
            }



        }





    }
}
public class Products
{
    public string Korona_ProductId { get; set; }
    public string article_name { get; set; }
    public string category { get; set; }
    public double retailPrice { get; set; }
}

public class PrductImages
{
    public string fileNameWithExtension { get; set; }
    public string fileNameWithoutExtension { get; set; }
}


