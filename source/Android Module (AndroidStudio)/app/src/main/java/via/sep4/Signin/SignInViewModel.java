package via.sep4.Signin;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import via.sep4.Model.AppData;
import via.sep4.Model.User;
import via.sep4.Persistence.PersistenceHandler;
import via.sep4.Persistence.WebHandler;

public class SignInViewModel extends ViewModel
{
    private PersistenceHandler persistenceHandler;
    private WebHandler webHandler;
    private MutableLiveData<User> user;
    private MutableLiveData<Boolean> success;

    public SignInViewModel() {
        persistenceHandler = new PersistenceHandler();
        webHandler = new WebHandler();
        User userInLiveData = new User();
        user = new MutableLiveData<>();
        user.setValue(userInLiveData);
        success = new MutableLiveData<>();
    }

    public MutableLiveData<User> getUser() {
        return user;
    }

    public MutableLiveData<Boolean> getSuccess() {return success;}

    //TODO: ensure that login only happens if connection successful
    public void SignIn()
    {
        webHandler.token(user.getValue(), new Callback<String>() {
            @Override
            public void onResponse(Call<String> call, Response<String> response) {
                AppData appData = AppData.getInstance();
                if(response.body() != null)
                {
                    appData.saveUser(user.getValue());
                    success.postValue(true);
                    /*String[] arr = appData.retrieveSavedUser();
                    if(arr[0].equals("") || arr[1].equals(""))
                    {
                        success.postValue(false);
                    }
                    else {
                        appData.saveUser(user.getValue());
                        success.postValue(true);
                    }*/
                }
            }

            @Override
            public void onFailure(Call<String> call, Throwable t) {

            }
        });
    }
}
