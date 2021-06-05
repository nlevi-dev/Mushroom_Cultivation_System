package via.sep4.Deprecated;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import via.sep4.Persistence.PersistenceHandler;
import via.sep4.Persistence.WebHandler;
@Deprecated
public class LightViewModel extends ViewModel {

    private MutableLiveData<Float> currentLightLiveData;
    private PersistenceHandler persistenceHandler;
    private WebHandler webHandler;
    private int specimenKey;
    private int hardwareID;

    public LightViewModel() {
        persistenceHandler = new PersistenceHandler();
        webHandler = new WebHandler();
        float f = 0;
        currentLightLiveData = new MutableLiveData<>();
        currentLightLiveData.setValue(f);
    }

    public MutableLiveData<Float> getCurrentLightLiveData()
    {
        return currentLightLiveData;
    }

    public void update()
    {
        String src = "light";
        currentLightLiveData.setValue(webHandler.getCurrentSensorData(hardwareID, src));
    }
}