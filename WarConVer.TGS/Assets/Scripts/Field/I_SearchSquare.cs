
//Strategyパターンのインターフェース(Commandパターンかも？)
//移動アルゴリズムクラスのインターフェース
interface I_SearchSquare { 

	int SearchSquare( int nowSquareIndex, Field.DIRECTION direction, int distance );

}