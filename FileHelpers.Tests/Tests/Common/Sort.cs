using System;
using System.Collections;
using System.IO;
using FileHelpers;
using NUnit.Framework;

namespace FileHelpersTests.CommonTests
{
	[TestFixture]
	public class SortRecords
	{
		FileHelperEngine engine;

		[Test]
		public void Sort1()
		{
			engine = new FileHelperEngine(typeof (CustomersVerticalBar));

			CustomersVerticalBar[] res =  engine.ReadFile(Common.TestPath(@"good\Sort1.txt")) as CustomersVerticalBar[];

			Assert.AreEqual(8, res.Length);

			CommonEngine.SortRecordsByField(res, "CompanyName");

			Assert.AreEqual(8, res.Length);

			Assert.AreEqual("Alfreds Futterkiste", res[0].CompanyName);
			Assert.AreEqual("La maison d'Asie", res[1].CompanyName);
			Assert.AreEqual("Tortuga Restaurante", res[2].CompanyName);
		}

		[Test]
		public void Sort2()
		{
			engine = new FileHelperEngine(typeof (CustomersSort));

			CustomersSort[] res =  engine.ReadFile(Common.TestPath(@"good\Sort1.txt")) as CustomersSort[];

			Assert.AreEqual(8, res.Length);

			CommonEngine.SortRecordsByField(res, "CompanyName");

			Assert.AreEqual(8, res.Length);

			Assert.AreEqual("Alfreds Futterkiste", res[0].CompanyName);
			Assert.AreEqual("La maison d'Asie", res[1].CompanyName);
			Assert.AreEqual("Tortuga Restaurante", res[2].CompanyName);
		}

		[Test]
		[ExpectedException(typeof(BadUsageException))]
		public void Sort3()
		{
			engine = new FileHelperEngine(typeof (CustomersVerticalBar));

			CustomersVerticalBar[] res =  engine.ReadFile(Common.TestPath(@"good\Sort1.txt")) as CustomersVerticalBar[];

			Assert.AreEqual(8, res.Length);

			CommonEngine.SortRecords(res);

			Assert.AreEqual(8, res.Length);

			Assert.AreEqual("Alfreds Futterkiste", res[0].CompanyName);
			Assert.AreEqual("La maison d'Asie", res[1].CompanyName);
			Assert.AreEqual("Tortuga Restaurante", res[2].CompanyName);
		}

		[Test]
		public void Sort4()
		{
			engine = new FileHelperEngine(typeof (CustomersSort));

			CustomersSort[] res =  engine.ReadFile(Common.TestPath(@"good\Sort1.txt")) as CustomersSort[];

			Assert.AreEqual(8, res.Length);

			CommonEngine.SortRecords(res);

			Assert.AreEqual(8, res.Length);

			Assert.AreEqual("Alfreds Futterkiste", res[0].CompanyName);
			Assert.AreEqual("La maison d'Asie", res[1].CompanyName);
			Assert.AreEqual("Tortuga Restaurante", res[2].CompanyName);
		}

		[Test]
		[ExpectedException(typeof(BadUsageException))]
		public void Sort5()
		{
			engine = new FileHelperEngine(typeof (CustomersSort));

			CustomersSort[] res =  engine.ReadFile(Common.TestPath(@"good\Sort1.txt")) as CustomersSort[];
			Assert.AreEqual(8, res.Length);

			CommonEngine.SortRecordsByField(res, "CompanyNameNoExistHere");
		}

		[DelimitedRecord("|")]
		private class CustomersSort: IComparable
		{
			public string CustomerID;
			public string CompanyName;
			public string ContactName;
			public string ContactTitle;
			public string Address;
			public string City;
			public string Country;

			#region IComparable Members

			public int CompareTo(object obj)
			{
				if (this == obj) return 0;
				
				CustomersSort to = (CustomersSort) obj;

				if (to == null) return int.MaxValue;

				return CompanyName.CompareTo(to.CompanyName);
			}

			#endregion
		}

	}
}