
//Strategyパターンのインターフェース(Commandパターンかも？)
//移動アルゴリズムクラスのインターフェース
interface ISearchSquare { 

	int SearchSquare( int nowSquareIndex, Field.DIRECTION direction, int distance );

}