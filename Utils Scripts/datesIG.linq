<Query Kind="Program" />

void Main()
{
	
}
private void ComputeLot(int idLot, List<int> list)
{
	DateTime startTime = new DateTime(2014, 5, 27).AddHours(18);
	Console.Write(startTime);
	foreach (int element in Lot1List)
	{
		Console.Write(startTime.AddHours(element));
	}

}
	
List<int> Lot1List = new List<int>{
    21,
    42,
    63,
    84,
    105,
    126,
    147,
    168,
    189,
    210,
    231,
    252,
    273,
    294,
    315,
    336,
    357,
    378,
    399,
    420,
    441,
    462,
    483,
    504,
    525,
    546,
    567,
    588,
    609,
    630,
    651,
    672,
    693,
    714,
    735,
    756,
    777,
    798,
    819,
    840,
    861,
    882,
    903,
    924,
    945,
    966,
    987,
    1008,
    1029,
    1050};
// Define other methods and classes here
    