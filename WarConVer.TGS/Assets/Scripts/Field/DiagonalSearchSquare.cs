
//斜め移動のアルゴリズムクラス
public class DiagonalSearchSquare : ISearchSquare {
	LeftAndRightSearchSquare left_and_right_search = new LeftAndRightSearchSquare( );
	ForwardAndBackSearchSquare forward_and_back_search = new ForwardAndBackSearchSquare( );


	public int SearchSquare( int now_square_index, Field.DIRECTION direction, int distance ) {
		int index = 0;

		//それぞれの斜め移動によって左右移動と上下移動のアルゴリズムクラスを使う----------------------------------------
		switch ( direction ) { 
			case Field.DIRECTION.LEFT_FORWARD:
				index = left_and_right_search.SearchSquare( now_square_index, Field.DIRECTION.LEFT, distance );
				if ( index == -1  ) return index;
				index = forward_and_back_search.SearchSquare( index, Field.DIRECTION.FORWAED, distance );
				return index;

			case Field.DIRECTION.RIGHT_FORWARD:
				index = left_and_right_search.SearchSquare( now_square_index, Field.DIRECTION.RIGHT, distance );
				if ( index == -1  ) return index;
				index = forward_and_back_search.SearchSquare( index, Field.DIRECTION.FORWAED, distance );
				return index;

			case Field.DIRECTION.LEFT_BACK:
				index = left_and_right_search.SearchSquare( now_square_index, Field.DIRECTION.LEFT, distance );
				if ( index == -1  ) return index;
				index = forward_and_back_search.SearchSquare( index, Field.DIRECTION.BACK, distance );
				return index;

			case Field.DIRECTION.RIGHT_BACK:
				index = left_and_right_search.SearchSquare( now_square_index, Field.DIRECTION.RIGHT, distance );
				if ( index == -1  ) return index;
				index = forward_and_back_search.SearchSquare( index, Field.DIRECTION.BACK, distance );
				return index;

			default:
				return -1;
		}
		//--------------------------------------------------------------------------------------------------------------
	}
}
