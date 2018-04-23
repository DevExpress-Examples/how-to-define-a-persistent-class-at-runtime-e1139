using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace CreateClassAtRuntime {
    class Program {
        static void Main(string[] args) {
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);

            XPDictionary dictionary = XpoDefault.DataLayer.Dictionary;
            XPClassInfo myBaseClass = dictionary.GetClassInfo(typeof(MyBaseObject));

            XPClassInfo myClassA = dictionary.CreateClass(myBaseClass, "MyObjectA");
            myClassA.CreateMember("ID", typeof(int), new KeyAttribute());
            myClassA.CreateMember("Name", typeof(string));

            XPCollection collection = new XPCollection(XpoDefault.Session, myClassA);
            Console.WriteLine("Objects loaded. Total count: {0}", collection.Count);
            foreach(MyBaseObject obj in collection) {
                Console.WriteLine("ID:\t{0}, Name:\t{1}", obj.GetMemberValue("ID"), obj.GetMemberValue("Name"));
            }
            Console.WriteLine("----------------------------");

            Console.WriteLine("Create a new object:");
            // generate a unique ID for a new object
            Random rnd = new Random();
            int newId;
            do {
                newId = rnd.Next(100);
            } while(XpoDefault.Session.GetObjectByKey(myClassA, newId) != null);

            MyBaseObject objNew = (MyBaseObject)myClassA.CreateNewObject(XpoDefault.Session);
            objNew.SetMemberValue("ID", newId);
            objNew.SetMemberValue("Name", String.Format("Name {0}", newId));
            objNew.Save();

            Console.WriteLine("ID:\t{0}, Name:\t{1}", objNew.GetMemberValue("ID"), objNew.GetMemberValue("Name"));

            Console.WriteLine("\nPress Enter to Exit");
            Console.ReadLine();
        }
    }

    [NonPersistent]
    public class MyBaseObject : XPLiteObject {
        public MyBaseObject(Session session)
            : base(session) { }
        public MyBaseObject(Session session, XPClassInfo classInfo)
            : base(session, classInfo) { }
    }
}
