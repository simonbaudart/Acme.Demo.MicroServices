docker build . -f Acme.Demo.MicroServices.Drawer\Dockerfile -t acme/demo/drawer:latest --target final
docker build . -f Acme.Demo.MicroServices.DrawerAdvanced\Dockerfile -t acme/demo/drawer-advanced:latest --target final
docker build . -f Acme.Demo.MicroServices.DrawerWavenet\Dockerfile -t acme/demo/drawer-wavenet:latest --target final
docker build . -f Acme.Demo.MicroServices.DrawingMentor\Dockerfile -t acme/demo/mentor:latest --target final
