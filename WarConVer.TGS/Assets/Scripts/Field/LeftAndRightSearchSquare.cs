
//左右移動のアルゴリズムクラス
public class LeftAndRightSearchSquare : I_SearchSquare {

	const int ONE_SQUIRREL = 1;	//左右に進むときの１マスの大きさ

	//それぞれの段の右端のマスのIndex
	const int FIRST_ROW_FIRST_INDEX  = 0;
	const int SECOND_ROW_FIRST_INDEX = 4;
	const int THIRD_ROW_FIRST_INDEX  = 8;
	const int FOURTH_ROW_FIRST_INDEX = 12;
	const int FIFTH_ROW_FIRST_INDEX  = 16;

	//それぞれの段の左端のマスのIndex
	const int FIRST_ROW_LAST_INDEX  = 3;
	const int SECOND_ROW_LAST_INDEX = 7;
	const int THIRD_ROW_LAST_INDEX  = 11;
	const int FOURTH_ROW_LAST_INDEX = 15;
	const int FIFTH_ROW_LAST_INDEX  = 19;

	const int ERROR = -1;

	public int SearchSquare( int nowSquareIndex, Field.DIRECTION direction, int distance ) {
		int index = 0;
		switch ( direction ) { 
			case Field.DIRECTION.LEFT:
				index = nowSquareIndex - ONE_SQUIRREL * distance;

				//今いる段の左端よりIndexが低くなったら-1を返す------------------------------------------------------------------------------------------------------
				if ( ( nowSquareIndex >= FIRST_ROW_FIRST_INDEX  && nowSquareIndex <= FIRST_ROW_LAST_INDEX  ) && index < FIRST_ROW_FIRST_INDEX  ) return ERROR;
				if ( ( nowSquareIndex >= SECOND_ROW_FIRST_INDEX && nowSquareIndex <= SECOND_ROW_LAST_INDEX ) && index < SECOND_ROW_FIRST_INDEX ) return ERROR;
				if ( ( nowSquareIndex >= THIRD_ROW_FIRST_INDEX  && nowSquareIndex <= THIRD_ROW_LAST_INDEX  ) && index < THIRD_ROW_FIRST_INDEX  ) return ERROR;
				if ( ( nowSquareIndex >= FOURTH_ROW_FIRST_INDEX && nowSquareIndex <= FOURTH_ROW_LAST_INDEX ) && index < FOURTH_ROW_FIRST_INDEX ) return ERROR;
				if ( ( nowSquareIndex >= FIFTH_ROW_FIRST_INDEX  && nowSquareIndex <= FIFTH_ROW_LAST_INDEX  ) && index < FIFTH_ROW_FIRST_INDEX  ) return ERROR;
				//---------------------------------------------------------------------------------------------------------------------------------------------------

				return index;

			case Field.DIRECTION.RIGHT:
				index = nowSquareIndex + ONE_SQUIRREL * distance;
				
				//今いる段の右端よりIndexが大きくなったら-1を返す------------------------------------------------------------------------------------------------------
				if ( ( nowSquareIndex >= FIRST_ROW_FIRST_INDEX  && nowSquareIndex <= FIRST_ROW_LAST_INDEX  ) && index > FIRST_ROW_LAST_INDEX  ) return ERROR;
				if ( ( nowSquareIndex >= SECOND_ROW_FIRST_INDEX && nowSquareIndex <= SECOND_ROW_LAST_INDEX ) && index > SECOND_ROW_LAST_INDEX ) return ERROR;
				if ( ( nowSquareIndex >= THIRD_ROW_FIRST_INDEX  && nowSquareIndex <= THIRD_ROW_LAST_INDEX  ) && index > THIRD_ROW_LAST_INDEX  ) return ERROR;
				if ( ( nowSquareIndex >= FOURTH_ROW_FIRST_INDEX && nowSquareIndex <= FOURTH_ROW_LAST_INDEX ) && index > FOURTH_ROW_LAST_INDEX ) return ERROR;
				if ( ( nowSquareIndex >= FIFTH_ROW_FIRST_INDEX  && nowSquareIndex <= FIFTH_ROW_LAST_INDEX  ) && index > FIFTH_ROW_LAST_INDEX  ) return ERROR;
				//---------------------------------------------------------------------------------------------------------------------------------------------------

				return index;

			default:
				return ERROR;
		}
	}
}
