using System;
using System.Diagnostics;
using System.Threading.Tasks;
//loop 
Console.Clear();
Console.Write("How many loops do you want to run? ");
string input = Console.ReadLine()!;

if(!int.TryParse(input, out int loopCount)|| loopCount <= 0){
    loopCount = 1;
    Console.WriteLine("Invalid input. Defaulting to 1 loop.");
}


//Cursor visible
Console.CursorVisible = false;


//working time
for (int i = 1 ; i<=  loopCount ; i++){
    bool completedWork = await RunTimer(1,$"WORK (Loop{i}/{loopCount})");

    if(!completedWork){
        Console.Clear();
        Console.WriteLine("Pomodoro loop interrupted. Exiting...");
        break;
    }
    if(i<loopCount){
         PlayMacSound($"Well done {i} loops completed. Now take a 5 minute break.");
        bool completedBreak = await RunTimer(5,$"BREAK(Loop{i}/{loopCount})");
        if(!completedBreak){
            Console.Clear();
            Console.WriteLine("Pomodoro loop interrupted. Exiting...");
            break;
        }
        PlayMacSound("Break is over. Time to get back to work!");
    }
    else{
        PlayMacSound("All pomodoro loops completed successfully! Great job!");
    }
}

Console.CursorVisible = true;
Console.WriteLine("\n🎉 Pomodoro loops completed successfully!\n");

static async Task<bool> RunTimer(int TotalSeconds , string sessionName){
    int remainingSeconds = TotalSeconds * 60;
    bool isPaused = false;
    DateTime lastTick = DateTime.Now;
    Console.Clear();

    while(remainingSeconds > 0){
        if(Console.KeyAvailable){
            var keyInfo = Console.ReadKey(intercept: true);
            if(keyInfo.Key == ConsoleKey.Spacebar){
                isPaused = !isPaused;
                if(!isPaused){
                    lastTick=DateTime.Now;
                }
            }
            else if (keyInfo.Key == ConsoleKey.Q){
                return false;
            }
        }
        if(!isPaused){
        TimeSpan elapsed = DateTime.Now - lastTick;
        if(elapsed.TotalSeconds >= 1){
            remainingSeconds--;
            lastTick = lastTick.AddSeconds(1);
        }
    }
    Console.SetCursorPosition(0,0);

    int m = remainingSeconds/60;
    int s = remainingSeconds%60;

    Console.WriteLine("--------------------------------------------------------");
    Console.WriteLine($"MOD:            {sessionName,-24}");
    Console.WriteLine("--------------------------------------------------------");

    if(isPaused){
        Console.WriteLine($"Time:         {m:D2}:{s:D2} ⏸️  STOPPED   ");
    }
    else{
        Console.WriteLine($"Time:         {m:D2}:{s:D2} ⏸️  STARTED   ");
    }
    Console.WriteLine("--------------------------------------------------------");
    Console.WriteLine($"        [Space] Start / Stop | [Q] Cancel");
    Console.WriteLine("--------------------------------------------------------");
    
    await Task.Delay(10);
    }
    return true;
}
static void PlayMacSound(string message){
    try{
        Console.Write("\a");
        Process.Start("say",message);
    }
    catch{}
}



    
    


