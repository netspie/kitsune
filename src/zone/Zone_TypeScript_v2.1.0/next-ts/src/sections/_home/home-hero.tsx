import { useRef } from 'react';

import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Unstable_Grid2';
import Typography from '@mui/material/Typography';
import { alpha, useTheme } from '@mui/material/styles';

import { paths } from 'src/routes/paths';
import Image from 'src/components/image';
import { bgGradient } from 'src/theme/css';
import Iconify from 'src/components/iconify';
import SvgColor from 'src/components/svg-color';
import { HEADER } from 'src/layouts/config-layout';
import { useResponsive } from 'src/hooks/use-responsive';
import { useBoundingClientRect } from 'src/hooks/use-bounding-client-rect';

// ----------------------------------------------------------------------

export default function HomeHero() {
  const theme = useTheme();

  const containerRef = useRef<HTMLDivElement>(null);

  const mdUp = useResponsive('up', 'md');

  const container = useBoundingClientRect(containerRef);

  const offsetLeft = container?.left;

  return (
    <Box
      sx={{
        ...bgGradient({
          color: alpha(theme.palette.background.default, 0.9),
          imgUrl: '/assets/background/overlay_1.jpg',
        }),
        overflow: 'hidden',
        position: 'relative',
        height: { md: `calc(100vh - ${HEADER.H_DESKTOP}px)` },
      }}
    >
      <Container sx={{ height: 1 }}>
        <Stack
          spacing={5}
          justifyContent="center"
          alignItems={{ xs: 'center', md: 'flex-center' }}
          sx={{
            py: 15,
            pt: 30,
            textAlign: { xs: 'center', md: 'left' },
          }}
        >
          <Typography variant="h1" textAlign={'center'}>
            Learn and Explore <br />
            <Box component="span" sx={{ color: 'primary.main' }}>
              {` Japanese`}
            </Box>
          </Typography>

          <Typography sx={{ color: 'text.secondary' }} textAlign={'center'}>
            The ultimate way to learn your favorite language. <br />
            Only thing you need.
          </Typography>

          <Button
            color="inherit"
            size="large"
            variant="contained"
            endIcon={<Iconify icon="carbon:launch" />}
            target="_blank"
            rel="noopener"
            href={paths.figmaPreview}
          >
            Get Started
          </Button>

          {/* <Stack spacing={3}>
                <Typography variant="overline">AVAILABLE FOR</Typography>
                <Stack direction="row" spacing={2.5}>
                  {['js', 'ts', 'figma', 'cra', 'nextjs'].map((icon) => (
                    <SvgColor
                      key={icon}
                      src={`/assets/icons/platforms/ic_${icon}.svg`}
                      sx={{ width: 24, height: 24 }}
                    />
                  ))}
                </Stack>
              </Stack> */}
        </Stack>

        <Grid xs={12} md={7}>
          <Box ref={containerRef} />
        </Grid>
      </Container>

      {/* {mdUp && (
        <Box
          sx={{
            maxWidth: 1280,
            position: 'absolute',
            bottom: { md: '20%', lg: 40 },
            right: { md: -110, xl: 0 },
            width: { md: `calc(100% - ${offsetLeft}px)` },
          }}
        >
          <Image
            visibleByDefault
            disabledEffect
            alt="home hero"
            src="/assets/images/home/home_hero.png"
          />
        </Box>
      )} */}
    </Box>
  );
}