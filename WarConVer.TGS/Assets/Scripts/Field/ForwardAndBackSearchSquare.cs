
//上下移動のアルゴリズムクラス
public class ForwardAndBackSearchSquare : ISearchSquare {
	const int ONE_SQUIRREL = 4;	//前後に進むときの１マスの大きさ

	//それぞれの段の右端のマスのIndex
	const int FIRST_ROW_FIRST_INDEX  = 1;
	const int SECOND_ROW_FIRST_INDEX = 5;
	const int THIRD_ROW_FIRST_INDEX  = 9;
	const int FOURTH_ROW_FIRST_INDEX = 13;
	const int FIFTH_ROW_FIRST_INDEX  = 17;

	//それぞれの段の左端のマスのIndex
	const int FIRST_ROW_LAST_INDEX  = 4;
	const int SECOND_ROW_LAST_INDEX = 8;
	const int THIRD_ROW_LAST_INDEX  = 12;
	const int FOURTH_ROW_LAST_INDEX = 16;
	const int FIFTH_ROW_LAST_INDEX  = 20;

	public int SearchSquare( int now_square_index, Field.DIRECTION direction, int distance ) {
		int index = 0;

		switch ( direction ) {
			case Field.DIRECTION.FORWAED:
				index = now_square_index - ONE_SQUIRREL * distance;
				
				//今いる段によってdistanceが一定以上多かったら-1を返す--------------------------------------------------------------------------
				if ( ( now_square_index >= FIRST_ROW_FIRST_INDEX  && now_square_index <= FIRST_ROW_LAST_INDEX  ) && distance > 0 ) return -1;
				if ( ( now_square_index >= SECOND_ROW_FIRST_INDEX && now_square_index <= SECOND_ROW_LAST_INDEX ) && distance > 1 ) return -1;
				if ( ( now_square_index >= THIRD_ROW_FIRST_INDEX  && now_square_index <= THIRD_ROW_LAST_INDEX  ) && distance > 2 ) return -1;
				if ( ( now_square_index >= FOURTH_ROW_FIRST_INDEX && now_square_index <= FOURTH_ROW_LAST_INDEX ) && distance > 3 ) return -1;
				if ( ( now_square_index >= FIFTH_ROW_FIRST_INDEX  && now_square_index <= FIFTH_ROW_LAST_INDEX  ) && distance > 4 ) return -1;
				//------------------------------------------------------------------------------------------------------------------------------
				
				return index;

			case Field.DIRECTION.BACK:
				index = now_square_index + ONE_SQUIRREL * distance;
				
				//今いる段によってdistanceが一定以上多かったら-1を返す--------------------------------------------------------------------------
				if ( ( now_square_index >= FIRST_ROW_FIRST_INDEX  && now_square_index <= FIRST_ROW_LAST_INDEX  ) && distance > 4 ) return -1;
				if ( ( now_square_index >= SECOND_ROW_FIRST_INDEX && now_square_index <= SECOND_ROW_LAST_INDEX ) && distance > 3 ) return -1;
				if ( ( now_square_index >= THIRD_ROW_FIRST_INDEX  && now_square_index <= THIRD_ROW_LAST_INDEX  ) && distance > 2 ) return -1;
				if ( ( now_square_index >= FOURTH_ROW_FIRST_INDEX && now_square_index <= FOURTH_ROW_LAST_INDEX ) && distance > 1 ) return -1;
				if ( ( now_square_index >= FIFTH_ROW_FIRST_INDEX  && now_square_index <= FIFTH_ROW_LAST_INDEX  ) && distance > 0 ) return -1;
				//------------------------------------------------------------------------------------------------------------------------------

				return index;

			default:
				return -1;
		}
	}

}
