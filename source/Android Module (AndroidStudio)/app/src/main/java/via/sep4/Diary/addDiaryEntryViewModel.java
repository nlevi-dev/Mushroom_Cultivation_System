package via.sep4.Diary;

import android.app.Application;

import androidx.lifecycle.AndroidViewModel;

import java.util.concurrent.Future;

import via.sep4.Persistence.DiaryEntryRepository;
/**
 * @author Bogdan Mezei
 * @version 1.0
 */
public class addDiaryEntryViewModel extends AndroidViewModel
{
	
	private final DiaryEntryRepository repository;
	
	public addDiaryEntryViewModel(Application application)
	{
		super(application);
		repository = DiaryEntryRepository.getInstance(application);
	}
	
	public void insert(final DiaryEntry entry)
	{
		repository.insert(entry);
	}
	
	public Future<DiaryEntry> getEntry(Integer id)
	{
		return repository.getEntry(id);
	}
	
	public void delete(DiaryEntry entry)
	{
		repository.delete(entry);
	}
}
