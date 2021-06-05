package sep4.iot.gateway;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.util.Collections;

/**
 * Main class for the SpringBoot based Web Application
 */
@SpringBootApplication
public class GatewayApplication {

    public static void main(String[] args) {
        SpringApplication app = new SpringApplication(GatewayApplication.class);
        app.setDefaultProperties(Collections.singletonMap("server.port", "41003"));
        app.run(args);
    }
}
