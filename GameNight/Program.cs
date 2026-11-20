/*
* Name: William Ayscue
* Course: CSCI 1250, Section 002
* Assignment: Lab 08, Game Night
* Date: November 20, 2026
* Description: Keeps a game night leaderboard in three text files. Shows the
* season so far, records tonight's points, saves the new totals,
* and adds tonight's result to a history that grows every week.
*/

//These direct the program where to put things when I use File.Append/Write
const string PLAYERS_FILE = "players.txt";
const string POINTS_FILE = "points.txt";
const string HISTORY_FILE = "history.txt";

// Look for the data files next to Program.cs, however the program is started.
string projectFolder = Path.GetFullPath(
Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
Directory.SetCurrentDirectory(projectFolder);

//This creates the needed arrays by looking in the players.txt and points.txt files
string[] players = File.ReadAllLines(PLAYERS_FILE);
string[] points = File.ReadAllLines(POINTS_FILE);
string[] history = File.ReadAllLines(HISTORY_FILE);
int night = history.Length + 1;

/*
I have put much of my program in methods. There is a detailed description above each method.
I have seperated the main body of my program into the three parts, but most of the stuff happens in my methods
*/

//Part 1: The standings. This prints out the season totals, and the history file
System.Console.WriteLine("=== GAME NIGHT LEADERBOARD ===");

//I used an if/else loop to check whether the history file has anything in it.
//I used this because I needed the program to decide between two actions. A while and a do/while will simply 
//stop the program. I needed it to check a condition once, make a decision and move on
if(history.Length == 0)
{
    System.Console.WriteLine("No nights played yet");
    System.Console.WriteLine("Tonight is night 1.");
}
else
{
    System.Console.WriteLine($"Nights played: {history.Length}.");
    
    //A foreach here instead of a for. I don't need to keep track of indexes or select a certain set from the array.
    //All I want is for it to regurgitate what it knows
    foreach(string line in history)
    {
        System.Console.WriteLine($"  {line}");
    }
    System.Console.WriteLine($"Tonight is night {history.Length + 1}.");
}

System.Console.WriteLine("");
PrintNight(players, points);
System.Console.WriteLine("");

//Part 2: The game night. This receives the new scores, adds them to the running total, and declares a winner for the night
int[] tonight = new int[points.Length];
//A for loop here because I am creating a new array using user input.
//A foreach would not work because it requires something to be in the array.
//I also needed to store certain numbers at certain locations
    for(int i = 0; i < points.Length; i++)
    {        
        System.Console.WriteLine($"Please enter {players[i]}'s score");
        System.Console.Write("Score must be between 1 and 20 ");
        tonight[i] = Convert.ToInt32(Console.ReadLine());
        
        //A while loop here because I need the program to refuse to budge.
        //An if statement would stop the program once and then move on like nothing happened, which would be unfortunate
        while(tonight[i] < 0 || tonight[i] > 20)
        {
            System.Console.Write("Please enter a score between 0 and 20 ");
            tonight[i] = Convert.ToInt32(Console.ReadLine());
        }        
    }
System.Console.WriteLine($"Tonight's Winner: {players[LeaderIndexTonight(tonight)]} with {tonight[LeaderIndexTonight(tonight)]}");

points = CalcSeason(points, tonight);

File.WriteAllLines(POINTS_FILE, points);

//Part 3: The history. This logs the events of the evening in the history file
string[] entry = { HistoryEntry(night, players, tonight) };
File.AppendAllLines(HISTORY_FILE, entry);


//This method finds the index position of the higest score no matter how many there are or where the score is.
//If I added a method that allowed the user to input extra people and scores, it would still work becuase,
//I used .Length instead of a number. If I had used a number, and the array length changed, my program crashes
static int LeaderIndexSeason(string[] points)
{
    int leaderPosition = 0;
    int leader = 0;
    
    //This loop is the most important. A foreach loop has no index indicator. 
    //Without it the program knows there is a leader, but it doesn't know where it is, 
    //which would make the program almost useless
    for(int i = 0; i < points.Length; i++)
    {
        int score = Convert.ToInt32(points[i]);

        //An if statement checks once and moves on. A while loop would work here only because I change the value of leader in the loop.
        //Using a while is a bad idea, because as soon as you get rid of lead = score, bad things happen 
        if(score > leader)
        {
            leader = score;
            leaderPosition = i;
        }
    }
    return leaderPosition;
}

//This method is esentially the same as the one above, except it is used for a different array of scores
static int LeaderIndexTonight(int[] tempTonightArray)
{
    int leaderPosition = 0;
    int leader = 0;

    //This loop is the most important. A foreach loop has no index indicator. 
    //Without it the program knows there is a leader, but it doesn't know where it is, 
    //which would make the program almost useless
    for(int i = 0; i < tempTonightArray.Length; i++)
    {
        int score = Convert.ToInt32(tempTonightArray[i]);
        //An if statement checks once and moves on. A while loop would work here only because I change the value of leader in the loop.
        //Using a while is a bad idea, because as soon as you get rid of lead = score, bad things happen
        if(score > leader)
        {
            leader = score;
            leaderPosition = i;
        }
    }
    return leaderPosition;
}

//This method calculates the total points no matter how many score entries there are.
//The foreach loop alows me to increase the array length without having to change the program
static int TotalPoints(string[] points)
{
    int pointTotal = 0;

    //Foreach because I don't need to keep track of location. I just need it to go through the array
    foreach(string point in points)
    {
       pointTotal = Convert.ToInt32(point) + pointTotal; 
    }
    return pointTotal;
}
//This method prints the night. There is no return type, it does not claculate a value, it only prints
static void PrintNight(string[] players, string[] points)
{
System.Console.WriteLine("Player".PadRight(10) + "Points");
//A for loop because I have to put a specific player with a specific point count. Without it Grace could get Alan's points,
//and Katherine could get Ada's, depending on how I wrote the program, which would defeat the whole purpose of the whole program
for(int i = 0; i < players.Length; i++)
{
    System.Console.WriteLine($"{players[i].PadRight(10)} {points[i]}");
}
System.Console.WriteLine($"Leader: {players[LeaderIndexSeason(points)]} with {points[LeaderIndexSeason(points)]}");
System.Console.WriteLine($"Season total: {TotalPoints(points)}");
}

//This adds each of tonight's scores to thier respective season scores.
static string[] CalcSeason(string[] points, int[] tonight)
{
    //A for loop beause I need to keep the sets of scores together. Ada's needs to be added to Ada's etc
    for(int i = 0; i < points.Length; i++)
    {
        int newPoint = tonight[i] + Convert.ToInt32(points[i]);
        points[i] = Convert.ToString(newPoint);
    }
    return points;
}

//This method returns a string variable that is ready to print to the history file.
//This takes most of the values that were calcualted from the given information and formats it acording to the lab specs
static string HistoryEntry(int night, string[] players, int[] points )
{
    string entryFile = $"Night {night}: {players[LeaderIndexTonight(points)]} won with {points[LeaderIndexTonight(points)]}";
    return entryFile;
}
