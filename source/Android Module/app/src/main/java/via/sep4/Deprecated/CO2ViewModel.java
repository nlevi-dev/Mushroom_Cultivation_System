package via.sep4.Deprecated;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import via.sep4.Persistence.PersistenceHandler;
import via.sep4.Persistence.WebHandler;
@Deprecated
public class CO2ViewModel extends ViewModel {
    private MutableLiveData<Float> currentCO2LiveData;
    private PersistenceHandler persistenceHandler;
    private WebHandler webHandler;
    private int specimenKey; //TODO: specimenkey and hardwareID passing
    private int hardwareID;

    public CO2ViewModel() {
        persistenceHandler = new PersistenceHandler();
        webHandler = new WebHandler();
        float f = 0;
        currentCO2LiveData = new MutableLiveData<>();
        currentCO2LiveData.setValue(f);
    }

    public MutableLiveData<Float> getCurrentLightLiveData()
    {
        return currentCO2LiveData;
    }

    public void update()
    {
        String src = "co2";
        currentCO2LiveData.setValue(webHandler.getCurrentSensorData(hardwareID, src));
    }
}