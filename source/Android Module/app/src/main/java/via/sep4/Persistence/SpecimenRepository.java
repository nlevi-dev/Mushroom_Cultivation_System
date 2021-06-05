package via.sep4.Persistence;

import android.app.Application;

import androidx.lifecycle.LiveData;

import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

import via.sep4.Model.SensorData;
import via.sep4.Model.Specimen;

public class SpecimenRepository {

    private static SpecimenRepository instance;
    private final PersistenceHandler.SpecimenDAO specimenDAO;
    private final LiveData<List<Specimen>> allSpecimens;
    private final ExecutorService executorService;

    private SpecimenRepository(Application application)
    {
        AppDatabase database = AppDatabase.getInstance(application);
        specimenDAO = database.specimenDAO();
        allSpecimens = specimenDAO.getAllSpecimens();
        executorService = Executors.newFixedThreadPool(10);
    }

    public static synchronized SpecimenRepository getInstance(Application application)
    {
        if (instance == null)
        {
            instance = new SpecimenRepository(application);
        }
        return instance;
    }
    public LiveData<List<Specimen>> getAllSpecimens()
    {
        return allSpecimens;
    }

    public Future<Specimen> getSpecimen(int specimenKey)
    {
        Callable<Specimen> call = () -> specimenDAO.getSpecimen(specimenKey);
        return executorService.submit(call);
    }

    public Future<List<SensorData>> getSensorDataBySpecimen(int specimenKey)
    {
        Callable<List<SensorData>> call = () -> specimenDAO.getSensorDataBySpecimen(specimenKey);
        return executorService.submit(call);
    }

    public void insertAll(Specimen... specimens)
    {
        executorService.execute(() -> specimenDAO.insertAll(specimens));
    }

    public void delete(Specimen specimen)
    {
        executorService.execute(() -> specimenDAO.delete(specimen));
    }
}
