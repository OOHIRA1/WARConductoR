
//Strategyパターンのインターフェース(Commandパターンかも？)
//移動アルゴリズムクラスのインターフェース
interface ISearchSquare { 

	int SearchSquare( int now_square_index, Field.DIRECTION direction, int distance );

}