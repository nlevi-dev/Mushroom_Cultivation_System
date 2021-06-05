package via.sep4.Diary;

import androidx.room.Entity;
import androidx.room.PrimaryKey;

import java.io.Serializable;
import java.util.Date;
/**
 * @author Bogdan Mezei
 * @version 1.0
 */
@Entity(tableName = "diary_table")
public class DiaryEntry implements Serializable
{
	@PrimaryKey(autoGenerate = true)
	private int id;
	
	private String entry;
	private Date dateAdded;

public DiaryEntry(String entry, Date dateAdded)
{
	this.entry = entry;
	this.dateAdded = dateAdded;
}

public int getId()
{
	return id;
}

public void setId(int id)
{
	this.id = id;
}

public String getEntry()
{
	return entry;
}

public void setEntry(String entry)
{
	this.entry = entry;
}

public Date getDateAdded()
{
	return dateAdded;
}

public void setDateAdded(Date dateAdded)
{
	this.dateAdded = dateAdded;
}
}
