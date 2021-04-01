using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GWorld
{
    private static GWorld instance;
    public bool IsPlay { get; set; }

    public bool RabbitIsLife { get; set; }
    public int CountSeconds = 60;

    public static GWorld Instance
    {
        get
        {
            if (instance is null)
                instance = new GWorld();
            return instance;
        }
    }
    private GWorld()
    {
        Reset();
    }

    public void Reset()
    {
        IsPlay = true;
        RabbitIsLife = true;
    }
}
