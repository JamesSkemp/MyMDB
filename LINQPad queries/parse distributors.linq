<Query Kind="Statements">
  <Connection>
    <ID>610c824c-dd33-4651-809a-3e2421ddaa56</ID>
    <Persist>true</Persist>
    <Server>JAMES-ASUS-X200\SQLEXPRESS</Server>
    <Database>MyMDb</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

var fileName = @"C:\Users\James\Downloads\Projects\iMDB\distributors.list";

if (File.Exists(fileName)) {

}


var reader = new StreamReader(fileName);

while (!reader.EndOfStream && reader.ReadLine() != "DISTRIBUTORS LIST")
{
}

for (int i = 0; i < 500; i++)
{
	var lineData = reader.ReadLine();
	if (string.IsNullOrWhiteSpace(lineData)) {
		continue;
	}

	string[] linearray = lineData.Split('\t');

	var distributor = new Distributor();
	distributor.RawData = lineData;
	distributor.Title = linearray[0];

	Distributors.InsertOnSubmit(distributor);
	SubmitChanges();


	lineData.Dump();
	var line = 0;
	foreach (var lineItem in linearray)
	{
		(++line).Dump();
		lineItem.Dump();
		"-".Dump();
	}
	"===".Dump();
	//reader.ReadLine().Dump();
}