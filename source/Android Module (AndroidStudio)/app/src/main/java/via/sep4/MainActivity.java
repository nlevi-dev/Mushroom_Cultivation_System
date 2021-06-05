package via.sep4;


import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.app.AppCompatDelegate;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.navigation.ui.AppBarConfiguration;
import androidx.navigation.ui.NavigationUI;

import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;

import com.google.android.material.bottomnavigation.BottomNavigationView;

import via.sep4.Dashboard.AddMushroomDialogFragment;
import via.sep4.Model.AppData;

public class MainActivity extends AppCompatActivity implements AddMushroomDialogFragment.AddMushroomDialogListener {
    private BottomNavigationView bottomNavigationView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_NO);
        AppData.setup(this);

        //Bottom Navigation Bar
        bottomNavigationView = (BottomNavigationView) findViewById(R.id.bottomNavigationView);
        //bottomNavigationView.setOnNavigationItemSelectedListener(navigationListener);
        bottomNavigationView.setVisibility(View.INVISIBLE); //To not see navigation bar on Sign In menu

        AppBarConfiguration appBarConfiguration = new AppBarConfiguration.Builder(R.id.fragment_info, R.id.dashboard).build();
        //avigation.setViewNavController(getCurrentFocus().findViewById(R.id.bottomNavigationView), new NavController(getApplicationContext()));
        NavController navController = Navigation.findNavController(this, R.id.fragmentbox);
        NavigationUI.setupActionBarWithNavController(this, navController, appBarConfiguration);
        NavigationUI.setupWithNavController(bottomNavigationView, navController);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        //getSupportFragmentManager().beginTransaction()
                //.replace(R.id.fragmentbox, new SignIn()).commit();
    }
    
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == android.R.id.home) {
            onBackPressed();
            return true;
        }
        return super.onOptionsItemSelected(item);
    }
    
    @Override
    public void onBackPressed() {
        if (getFragmentManager().getBackStackEntryCount() > 0 ) {
            getFragmentManager().popBackStack();
        }
        else {
            super.onBackPressed();
        }
    }
}