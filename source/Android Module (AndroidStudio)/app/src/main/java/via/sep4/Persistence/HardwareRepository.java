package via.sep4.Persistence;

import android.app.Application;

import androidx.lifecycle.LiveData;

import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

import via.sep4.Model.Hardware;
import via.sep4.Model.Specimen;
import via.sep4.Model.Status;

public class HardwareRepository {
    private static HardwareRepository instance;
    private final PersistenceHandler.HardwareDAO hardwareDAO;
    private final LiveData<List<Hardware>> allHardware;
    private final ExecutorService executorService;

    private HardwareRepository(Application application)
    {
        AppDatabase database = AppDatabase.getInstance(application);
        hardwareDAO = database.hardwareDAO();
        allHardware = hardwareDAO.getHardwareList();
        executorService = Executors.newFixedThreadPool(10);
    }

    public static synchronized HardwareRepository getInstance(Application application)
    {
        if (instance == null)
        {
            instance = new HardwareRepository(application);
        }
        return instance;
    }

    public LiveData<List<Hardware>> getAllHardware()
    {
        return allHardware;
    }

    public Future<Hardware> getHardware(int hardwareKey)
    {
        Callable<Hardware> call = () -> hardwareDAO.getHardware(hardwareKey);
        return executorService.submit(call);
    }

    public void insertAll(Hardware... hardwares)
    {
        executorService.execute(() -> hardwareDAO.insertAll(hardwares));
    }

    public void delete(Hardware hardware)
    {
        executorService.execute(() -> hardwareDAO.delete(hardware));
    }

}
