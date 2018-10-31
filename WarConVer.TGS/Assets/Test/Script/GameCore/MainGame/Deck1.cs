using UnityEngine;

public class Deck1 : MonoBehaviour {

    public const int MAX_DECK_NUM = 20;
    const int MAX_CARD_ID_NUM = (int)CardList.Card.MAX_CARD_NUM;
    CardList CardList;
    [SerializeField] int[] Conciliator_Deck;
    [SerializeField] int[] Immortality_Deck;
    [SerializeField] bool Ledy_Deck = false;
    [SerializeField] int deck_list_num = 0;
    [SerializeField] string Deck_Name = "";
    [SerializeField] enum DECK_LIST
    {
        Conciliator,
        Immortality,
        MAX_DECK_LIST
    };
    private DECK_LIST GetDeck;
    public int Deck_Select_Num
    {
        set { deck_list_num = value; }
        get { return deck_list_num; }
    }
    public string deck_name
    {
        set { Deck_Name = value; }
        get { return Deck_Name; }
    }
    public bool Get_Ledy_Deck
    {
        set { Ledy_Deck = value; }
        get { return Ledy_Deck; }
    }
    public int[] Deck = new int [MAX_DECK_NUM];

    // Use this for initialization
    private void Start()
    {
    
        Conciliator_Deck = new int[MAX_DECK_NUM]
        {
            //Volsunga,Wegweiserは仮
           (int)CardList.Card.Carbuncle,
           (int)CardList.Card.Carbuncle,
           (int)CardList.Card.Carbuncle,
           (int)CardList.Card.kmiusagiaa,
           (int)CardList.Card.kmiusagiaa,
           (int)CardList.Card.kmiusagiaa,
           (int)CardList.Card.blackdragon,
           (int)CardList.Card.blackdragon,
           (int)CardList.Card.blackdragon,
           (int)CardList.Card.garuda,
           (int)CardList.Card.garuda,
           (int)CardList.Card.garuda,
           (int)CardList.Card.kennryuu,
           (int)CardList.Card.kennryuu,
           (int)CardList.Card.Volsunga,
           (int)CardList.Card.Volsunga,
           (int)CardList.Card.Volsunga,
           (int)CardList.Card.Wegweiser,
           (int)CardList.Card.Wegweiser,
           (int)CardList.Card.Wegweiser,
           
        };

        Immortality_Deck = new int[MAX_DECK_NUM]
        {
           (int)CardList.Card.karasu,
           (int)CardList.Card.karasu,
           (int)CardList.Card.zombie,
           (int)CardList.Card.zombie,
           (int)CardList.Card.zombie,
           (int)CardList.Card.undead,
           (int)CardList.Card.undead,
           (int)CardList.Card.dracula,
           (int)CardList.Card.cornius,
           (int)CardList.Card.cornius,
           (int)CardList.Card.cornius,
           (int)CardList.Card.n,
           (int)CardList.Card.n,
           (int)CardList.Card.machine,
           (int)CardList.Card.machine,
           (int)CardList.Card.machine,
           (int)CardList.Card.suke,
           (int)CardList.Card.suke,
           (int)CardList.Card.suke,
           (int)CardList.Card.n,
        };
        Ledy_Deck = true;
    }
    public void Deck_Select()
    {
       
        switch (Deck_Name)
        {
            case "Conciliator":
                Deck = Conciliator_Deck;
                Immortality_Deck = null;
                Conciliator_Deck = null;

                break;
            case "Immortality":
                Deck = Immortality_Deck;
                Conciliator_Deck = null;
                Immortality_Deck = null;
                break;
            default:
                break;
        }

    }

}
	
		
