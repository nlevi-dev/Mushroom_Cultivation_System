package via.sep4.Persistence;

import androidx.room.TypeConverter;

import java.util.Date;
/**
 * @author Bogdan Mezei
 * @version 1.0
 */
public class Converters
{
	
	@TypeConverter
	public static Date fromTimestamp(Long value)
	{
		return value == null ? null : new Date(value);
	}
	
	@TypeConverter
	public static Long dateToTimestamp(Date date)
	{
		return date == null ? null : date.getTime();
	}
	
}