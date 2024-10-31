// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using game1402_a2_starter;

string fileName = "../../../game_data.json";//if you are ever worried about whether your json is valid or not, check out JSON Lint: 

GameData yourGameData;
string jsonString = File.ReadAllText(@fileName);
yourGameData = JsonSerializer.Deserialize<GameData>(jsonString);
Game yourGame = new Game(yourGameData);

yourGame.Init(); //initialize game

while (!yourGame.gameIsWon)
{
    Console.Write("\n>");//for formatting purposes
    yourGame.ProcessString(Console.ReadLine());
}

