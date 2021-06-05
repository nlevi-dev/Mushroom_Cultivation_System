package via.sep4.Persistence;

import android.app.Application;

import androidx.lifecycle.LiveData;

import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

import via.sep4.Diary.DiaryEntry;

/**
 * @author Bogdan Mezei
 * @version 1.0
 */
public class DiaryEntryRepository
{
	private static DiaryEntryRepository instance;
	private final PersistenceHandler.DiaryEntryDAO entryDAO;
	private final LiveData<List<DiaryEntry>> allEntries;
	private final ExecutorService executorService;
	
	private DiaryEntryRepository(Application application)
	{
		AppDatabase database = AppDatabase.getInstance(application);
		entryDAO = database.diaryEntryDAO();
		allEntries = entryDAO.getAllEntries();
		executorService = Executors.newFixedThreadPool(10);
	}
	
	public static synchronized DiaryEntryRepository getInstance(Application application)
	{
		if (instance == null)
		{
			instance = new DiaryEntryRepository(application);
		}
		return instance;
	}
	
	public LiveData<List<DiaryEntry>> getAllEntries()
	{
		return allEntries;
	}
	
	public void insert(final DiaryEntry entry)
	{
		executorService.execute(() -> entryDAO.insert(entry));
	}
	
	
	public Future<DiaryEntry> getEntry(Integer id)
	{
		Callable<DiaryEntry> call = () -> entryDAO.getDiaryEntry(id);
		return executorService.submit(call);
	}
	
	
	public void delete(DiaryEntry entry)
	{
		executorService.execute(() -> entryDAO.delete(entry));
	}
	
}
