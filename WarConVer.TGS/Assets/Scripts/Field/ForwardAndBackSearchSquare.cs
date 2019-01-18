
//上下移動のアルゴリズムクラス
public class ForwardAndBackSearchSquare : I_SearchSquare {
	const int ONE_SQUIRREL = 4;	//前後に進むときの１マスの大きさ

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
			case Field.DIRECTION.FORWAED:
				index = nowSquareIndex - ONE_SQUIRREL * distance;
				
				//今いる段によってdistanceが一定以上多かったら-1を返す-------------------------------------------------------------------------------------------------------------------------
				if ( ( nowSquareIndex >= firstRowFirstIndex  && nowSquareIndex <= firstRowLastIndex  ) && distance > ConstantStorehouse.FIRST_ROW_INDEX  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= secondRowFirstIndex && nowSquareIndex <= secondRowLastIndex ) && distance > ConstantStorehouse.SECOND_ROW_INDEX ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= thirdRowFirstIndex  && nowSquareIndex <= thirdRowLastIndex  ) && distance > ConstantStorehouse.THIRD_ROW_INDEX  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fourthRowFirstIndex && nowSquareIndex <= fourthRowLastIndex ) && distance > ConstantStorehouse.FOURTH_ROW_INDEX ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fifthRowFirstIndex  && nowSquareIndex <= fifthRowLastIndex  ) && distance > ConstantStorehouse.FIFTH_ROW_INDEX  ) return ConstantStorehouse.ERROR;
				//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
				
				return index;

			case Field.DIRECTION.BACK:
				index = nowSquareIndex + ONE_SQUIRREL * distance;
				
				//今いる段によってdistanceが一定以上多かったら-1を返す------------------------------------------------------------------------------------------------------------------------
				if ( ( nowSquareIndex >= firstRowFirstIndex  && nowSquareIndex <= firstRowLastIndex  ) && distance > ConstantStorehouse.FIFTH_ROW_INDEX  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= secondRowFirstIndex && nowSquareIndex <= secondRowLastIndex ) && distance > ConstantStorehouse.FOURTH_ROW_INDEX ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= thirdRowFirstIndex  && nowSquareIndex <= thirdRowLastIndex  ) && distance > ConstantStorehouse.THIRD_ROW_INDEX  ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fourthRowFirstIndex && nowSquareIndex <= fourthRowLastIndex ) && distance > ConstantStorehouse.SECOND_ROW_INDEX ) return ConstantStorehouse.ERROR;
				if ( ( nowSquareIndex >= fifthRowFirstIndex  && nowSquareIndex <= fifthRowLastIndex  ) && distance > ConstantStorehouse.FIRST_ROW_INDEX  ) return ConstantStorehouse.ERROR;
				//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

				return index;

			default:
				return ConstantStorehouse.ERROR;
		}
	}

}
