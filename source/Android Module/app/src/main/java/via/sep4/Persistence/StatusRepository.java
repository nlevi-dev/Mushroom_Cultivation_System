package via.sep4.Persistence;

import android.app.Application;

import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

import via.sep4.Model.Status;

public class StatusRepository {
    private static StatusRepository instance;
    private final PersistenceHandler.StatusDAO statusDAO;
    private final ExecutorService executorService;

    private StatusRepository(Application application)
    {
        AppDatabase database = AppDatabase.getInstance(application);
        statusDAO = database.statusDAO();
        executorService = Executors.newFixedThreadPool(10);
    }

    public static synchronized StatusRepository getInstance(Application application)
    {
        if (instance == null)
        {
            instance = new StatusRepository(application);
        }
        return instance;
    }

    public Future<List<Status>> getStatusListBySpecimen(int specimenKey)
    {
        Callable<List<Status>> call = () -> statusDAO.getAllBySpecimen(specimenKey);
        return executorService.submit(call);
    }

    public void insertAll(Status... status)
    {
        executorService.execute(() -> statusDAO.insertAll(status));
    }

    public void delete(Status status)
    {
        executorService.execute(() -> statusDAO.delete(status));
    }

}
