Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.Xpo
Imports DevExpress.Xpo.Metadata

Namespace CreateClassAtRuntime
	Friend Class Program
		Shared Sub Main(ByVal args() As String)
			XpoDefault.DataLayer = XpoDefault.GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema)

			Dim dictionary As XPDictionary = XpoDefault.DataLayer.Dictionary
			Dim myBaseClass As XPClassInfo = dictionary.GetClassInfo(GetType(MyBaseObject))

			Dim myClassA As XPClassInfo = dictionary.CreateClass(myBaseClass, "MyObjectA")
			myClassA.CreateMember("ID", GetType(Integer), New KeyAttribute())
			myClassA.CreateMember("Name", GetType(String))

			Dim collection As New XPCollection(XpoDefault.Session, myClassA)
			Console.WriteLine("Objects loaded. Total count: {0}", collection.Count)
			For Each obj As MyBaseObject In collection
				Console.WriteLine("ID:" & Constants.vbTab & "{0}, Name:" & Constants.vbTab & "{1}", obj.GetMemberValue("ID"), obj.GetMemberValue("Name"))
			Next obj
			Console.WriteLine("----------------------------")

			Console.WriteLine("Create a new object:")
			' generate a unique ID for a new object
			Dim rnd As New Random()
			Dim newId As Integer
			Do
				newId = rnd.Next(100)
			Loop While XpoDefault.Session.GetObjectByKey(myClassA, newId) IsNot Nothing

			Dim objNew As MyBaseObject = CType(myClassA.CreateNewObject(XpoDefault.Session), MyBaseObject)
			objNew.SetMemberValue("ID", newId)
			objNew.SetMemberValue("Name", String.Format("Name {0}", newId))
			objNew.Save()

			Console.WriteLine("ID:" & Constants.vbTab & "{0}, Name:" & Constants.vbTab & "{1}", objNew.GetMemberValue("ID"), objNew.GetMemberValue("Name"))

			Console.WriteLine(Constants.vbLf & "Press Enter to Exit")
			Console.ReadLine()
		End Sub
	End Class

	<NonPersistent> _
	Public Class MyBaseObject
		Inherits XPLiteObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Sub New(ByVal session As Session, ByVal classInfo As XPClassInfo)
			MyBase.New(session, classInfo)
		End Sub
	End Class
End Namespace
