using Data.DataBase;
using Data.DataBase.Tables;

var db = new DataBase<Human>();
var human = new Human{Name = "Test", Surname = "TestSurname"};
db.Insert(human);
var humans = db.GetAll().ToArray();
Array.ForEach(humans,Console.WriteLine);