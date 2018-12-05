using UnityEngine;
using System.Linq;

public class CreatedCard : CardList {

    public int CardNum = 0;
    [SerializeField] int CardMoveType = 0;
//    [SerializeField] string Name = "";
    [SerializeField] int[ ] Num = new int[ 3 ];

    // public int CardNum { get { return _CardNum; } set { _CardNum = value; } }

    /// <summary>
    /// GameMgrからの呼び出しがあったときのみカードのデータを作る
    /// </summary>
    /// <param CardList Card="Data"></param>
    /// <param GameObject="Card"></param>
    public delegate void CreatCallBack( int move, int[ ] Status);

//    [SerializeField] private int SearchCard = ( int )( ( int )( Card.MAX_CARD_NUM ) / 2 );


    public void  Created ( CreatCallBack creatCall ) 
    {

        //本番は外部データ参照のためこの下の配列は消える

        var Data = new[ ]
      {
            new{ num = Card.Vanargand,    move = Move.poan    },//0
            new{ num = Card.Hohenstaufen, move = Move.poan    },//1
            new{ num = Card.Schwarzwald,  move = Move.poan    },//2
            new{ num = Card.Volsunga,     move = Move.poan    },//3
            new{ num = Card.Nachzehrer,   move = Move.poan    },//4
            new{ num = Card.Luzifer,      move = Move.poan    },//5
            new{ num = Card.Tobalcaine,   move = Move.poan    },//6
            new{ num = Card.Pegasus,      move = Move.poan    },//7
            new{ num = Card.VereFilius,   move = Move.poan    },//8
            new{ num = Card.Babylon,      move = Move.poan    },//9
            new{ num = Card.Heilige,      move = Move.poan    },//10
            new{ num = Card.Wegweiser,    move = Move.poan    },//11
            new{ num = Card.Swastika,     move = Move.poan    },//12
            new{ num = Card.Guilltine,    move = Move.poan    },//13
            new{ num = Card.Rusalka,      move = Move.poan    },//14
            new{ num = Card.dracula,      move = Move.poan    },//15
            new{ num = Card.undead,       move = Move.poan  },//16
            new{ num = Card.blackdragon,  move = Move.poan  },//17
            new{ num = Card.kmiusagiaa,   move = Move.poan  },//18
            new{ num = Card.suke,         move = Move.poan  },//19
            new{ num = Card.zombie,       move = Move.poan  },//20
            new{ num = Card.Carbuncle,    move = Move.poan  },//21
            new{ num = Card.garuda,       move = Move.poan  },//22
            new{ num = Card.kennryuu,     move = Move.poan  },//23
            new{ num = Card.karasu,       move = Move.poan  },//24
            new{ num = Card.suke2,        move = Move.poan  },//24
            new{ num = Card.samechang,    move = Move.poan  },//24
            new{ num = Card.reaper,       move = Move.poan  },//24
            new{ num = Card.poseidonn,    move = Move.poan  },//24
            new{ num = Card.kettosi,      move = Move.poan  },//24
            new{ num = Card.cornius,      move = Move.poan  },//25
            new{ num = Card.gypsum,       move = Move.poan  },//28
            new{ num = Card.machine,      move = Move.poan},//29
            new{ num = Card.n,            move = Move.poan    },//27
        };

        
        //status 左から Com Pow Cost EFFECT Move
        var palam = new[ ]
        {
           new { num = 0,  Status = new[ ]{ 10, 9, 8, 0, 2    } },
           new { num = 1,  Status = new[ ]{  9,10, 7, 0, 3    } },
           new { num = 2,  Status = new[ ]{  4, 7, 7, 0, 2    } },
           new { num = 3,  Status = new[ ]{  2, 5, 3, 0, 2    } },
           new { num = 4,  Status = new[ ]{  5, 3, 4, 0, 2    } },
           new { num = 5,  Status = new[ ]{  3, 2, 1, 0, 1    } },
           new { num = 6,  Status = new[ ]{  6, 7, 5, 0, 2    } },
           new { num = 7,  Status = new[ ]{  7, 7, 6, 0, 2    } },
           new { num = 8,  Status = new[ ]{  9, 8, 8, 0, 2    } },
           new { num = 9,  Status = new[ ]{ 10,10,10, 0, 3    } },
           new { num = 10, Status = new[ ]{  9, 7, 7, 0, 2    } },
           new { num = 11, Status = new[ ]{  4, 5, 4, 0, 2    } },
           new { num = 12, Status = new[ ]{  3, 2, 2, 0, 1    } },
           new { num = 13, Status = new[ ]{  3, 4, 4, 0, 1    } },
           new { num = 14, Status = new[ ]{  2, 3, 1, 0, 1    } },
           new { num = 15, Status = new[ ]{  8, 5, 8, 4, 2    } },
           new { num = 16, Status = new[ ]{  5, 7, 5, 0, 2    } },
           new { num = 17, Status = new[ ]{  5, 4, 6, 3, 2    } },
           new { num = 18, Status = new[ ]{  3, 2, 3, 0, 1    } },
           new { num = 19, Status = new[ ]{  4, 1, 5, 3, 2    } },
           new { num = 20, Status = new[ ]{  2, 3, 2, 0, 1    } },
           new { num = 21, Status = new[ ]{  3, 2, 2, 0, 1    } },
           new { num = 22, Status = new[ ]{  6, 7, 6, 0, 2    } },
           new { num = 23, Status = new[ ]{  9, 8, 8, 0, 2    } },
           new { num = 24, Status = new[ ]{  6, 6, 6, 3, 2    } },
           new { num = 25, Status = new[ ]{  5, 4, 5, 3, 2    } },
           new { num = 26, Status = new[ ]{  5, 4, 5, 3, 2    } },
           new { num = 27, Status = new[ ]{  5, 4, 5, 3, 2    } },
           new { num = 28, Status = new[ ]{  5, 4, 5, 3, 2    } },
           new { num = 29, Status = new[ ]{  5, 4, 5, 3, 2    } },
           new { num = 30, Status = new[ ]{  5, 4, 5, 3, 2    } },
           new { num = 31, Status = new[ ]{  2, 2, 3, 2, 2    } },
           new { num = 32, Status = new[ ]{  6, 6, 6, 3, 2    } },
           new { num = 33, Status = new[ ]{  4, 2, 3, 0, 1    } },

        };


        var MoveType = Data
            .Where( c => ( int )c.num == CardNum )
            .Select( c => c.move );
        foreach ( var c in MoveType )
        {
            CardMoveType = ( int )c;
        }

        var Palam = palam
            .Where( p => p.num == CardNum )
            .Select( p => p.Status );
        foreach( var p in Palam )
        {
            Num = p;
        }
        creatCall( CardMoveType,Num );
    }

}
