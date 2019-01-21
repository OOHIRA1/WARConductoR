
//左右移動のアルゴリズムクラス
public class LeftAndRightSearchSquare : I_SearchSquare {
	const int ONE_SQUIRREL = 1;	//左右に進むときの１マスの大きさ

	public int SearchSquare( int nowSquareIndex, Field.DIRECTION direction, int distance ) {
		int index = 0;

		//それぞれの段の右端のマスのIndex
		int firstRowFirstIndex  = ConstantStorehouse.SQUARE_ROW_NUM * ConstantStorehouse.FIRST_ROW_INDEX;
		int secondRowFirstIndex = ConstantStorehouse.SQUARE_ROW_NUM * ConstantStorehouse.SECOND_ROW_INDEX;
		int thirdRowFirstIndex  = ConstantStorehouse.SQUARE_ROW_NUM * ConstantStorehouse.THIRD_ROW_INDEX;
		int fourthRowFirstIndex = ConstantStorehouse.SQUARE_ROW_NUM * ConstantStorehouse.FOURTH_ROW_INDEX;
		int fifthRowFirstIndex  = ConstantStorehouse.SQUARE_ROW_NUM * ConstantStorehouse.FIFTH_ROW_INDEX;

		//それぞれの段の左端のマスのIndex
		int firstRowLastIndex  = firstRowFirstIndex  + ( ConstantStorehouse.SQUARE_ROW_NUM - 1 );
		int secondRowLastIndex = secondRowFirstIndex + ( ConstantStorehouse.SQUARE_ROW_NUM - 1 );
		int thirdRowLastIndex  = thirdRowFirstIndex  + ( ConstantStorehouse.SQUARE_ROW_NUM - 1 );
		int fourthRowLastIndex = fourthRowFirstIndex + ( ConstantStorehouse.SQUARE_ROW_NUM - 1 );
		int fifthRowLastIndex  = fifthRowFirstIndex  + ( ConstantStorehouse.SQUARE_ROW_NUM - 1 );

		switch ( direction ) { 
			case Field.DIRECTION.LEFT:
				index = nowSquareIndex - ONE_SQUIRREL * distance;

				//今いる段の左端よりIndexが低くなったら-1を返す---------------------------------------------------------------------------------------------------------
				if ( ( nowSquareIndex >= firstRowFirstIndex  && nowSquareIndex <= firstRowLastIndex  ) && index < firstRowFirstIndex  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= secondRowFirstIndex && nowSquareIndex <= secondRowLastIndex ) && index < secondRowFirstIndex ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= thirdRowFirstIndex  && nowSquareIndex <= thirdRowLastIndex  ) && index < thirdRowFirstIndex  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fourthRowFirstIndex && nowSquareIndex <= fourthRowLastIndex ) && index < fourthRowFirstIndex ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fifthRowFirstIndex  && nowSquareIndex <= fifthRowLastIndex  ) && index < fifthRowFirstIndex  ) return ConstantStorehouse.ERROR;
				//------------------------------------------------------------------------------------------------------------------------------------------------------

				return index;

			case Field.DIRECTION.RIGHT:
				index = nowSquareIndex + ONE_SQUIRREL * distance;
				
				//今いる段の右端よりIndexが大きくなったら-1を返す------------------------------------------------------------------------------------------------------
				if ( ( nowSquareIndex >= firstRowFirstIndex  && nowSquareIndex <= firstRowLastIndex  ) && index > firstRowLastIndex  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= secondRowFirstIndex && nowSquareIndex <= secondRowLastIndex ) && index > secondRowLastIndex ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= thirdRowFirstIndex  && nowSquareIndex <= thirdRowLastIndex  ) && index > thirdRowLastIndex  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fourthRowFirstIndex && nowSquareIndex <= fourthRowLastIndex ) && index > fourthRowLastIndex ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fifthRowFirstIndex  && nowSquareIndex <= fifthRowLastIndex  ) && index > fifthRowLastIndex  ) return ConstantStorehouse.ERROR;
				//-----------------------------------------------------------------------------------------------------------------------------------------------------

				return index;

			default:
				return ConstantStorehouse.ERROR;
		}
	}
}
