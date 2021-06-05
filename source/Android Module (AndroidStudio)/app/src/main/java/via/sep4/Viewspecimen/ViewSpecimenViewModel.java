package via.sep4.Viewspecimen;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import via.sep4.Model.SensorDataList;
import via.sep4.Model.Specimen;
import via.sep4.Persistence.PersistenceHandler;
import via.sep4.Persistence.WebHandler;

public class ViewSpecimenViewModel extends ViewModel
{
    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class is responsible for implementing UI background code.
     */

    private PersistenceHandler persistenceHandler;
    private final ExecutorService service;
    private WebHandler webHandler;
    private int specimenKey;
    private MutableLiveData<SensorDataList> sensorLiveData;
    //TODO: visualizer, visualizer data binding

    public ViewSpecimenViewModel()
    {
        persistenceHandler = new PersistenceHandler();
        webHandler = new WebHandler();
        service = Executors.newFixedThreadPool(2);
        sensorLiveData = new MutableLiveData<>();
        specimenKey = 0;
        //TODO: write specimen key retriever
    }

    public MutableLiveData<SensorDataList> getSensorLiveData() {
        return sensorLiveData;
    }

    /*private void setSensorLiveData()
    {
        sensorLiveData.setValue(getSensorData());
    }*/

    private void addSensorLiveData()
    {

    }

    public Specimen getSpecimen()
    {
        //TODO: write internet checker
        return webHandler.getSpecimen(specimenKey);
    }

    public SensorDataList getLocalSensorData()
    {
        return null;
        //return persistenceHandler.getAllSensorData(specimenKey);
    }
    
//    public void getSensorData() {
//        service.execute(() -> {
//
//            sensorLiveData.postValue(webHandler.getSensorDataList(specimenKey));
//            try
//            {
//                Thread.sleep(300000);
//            } catch (InterruptedException e)
//            {
//                e.printStackTrace();
//            }
//        });
//
//    }

}
