
//斜め移動のアルゴリズムクラス
public class DiagonalSearchSquare : I_SearchSquare {
	LeftAndRightSearchSquare leftAndRightSearch = new LeftAndRightSearchSquare( );
	ForwardAndBackSearchSquare forwardAndBackSearch = new ForwardAndBackSearchSquare( );

	const int ERROR = -1;

	public int SearchSquare( int nowSquareIndex, Field.DIRECTION direction, int distance ) {
		int index = 0;

		//それぞれの斜め移動によって左右移動と上下移動のアルゴリズムクラスを使う----------------------------------------
		switch ( direction ) { 
			case Field.DIRECTION.LEFT_FORWARD:
				index = leftAndRightSearch.SearchSquare( nowSquareIndex, Field.DIRECTION.LEFT, distance );
				if ( index == ERROR ) return index;
				index = forwardAndBackSearch.SearchSquare( index, Field.DIRECTION.FORWAED, distance );
				return index;

			case Field.DIRECTION.RIGHT_FORWARD:
				index = leftAndRightSearch.SearchSquare( nowSquareIndex, Field.DIRECTION.RIGHT, distance );
				if ( index == ERROR ) return index;
				index = forwardAndBackSearch.SearchSquare( index, Field.DIRECTION.FORWAED, distance );
				return index;

			case Field.DIRECTION.LEFT_BACK:
				index = leftAndRightSearch.SearchSquare( nowSquareIndex, Field.DIRECTION.LEFT, distance );
				if ( index == ERROR ) return index;
				index = forwardAndBackSearch.SearchSquare( index, Field.DIRECTION.BACK, distance );
				return index;

			case Field.DIRECTION.RIGHT_BACK:
				index = leftAndRightSearch.SearchSquare( nowSquareIndex, Field.DIRECTION.RIGHT, distance );
				if ( index == ERROR ) return index;
				index = forwardAndBackSearch.SearchSquare( index, Field.DIRECTION.BACK, distance );
				return index;

			default:
				return ERROR;
		}
		//--------------------------------------------------------------------------------------------------------------
	}
}
